using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.Constants;
using WebAPI.DTOs;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repositories;
using static WebAPI.Constants.Permissions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private IAttendence attendenceRepo;
        private IEmployeeRepo employeeRepo;
        private IMapper mapper;
        private IDaysOffRepo daysOffRepo;
        private IWeeklyDaysOff weeklyDaysOffRepo;

        public AttendenceController(IAttendence attendenceRepo, IEmployeeRepo employeeRepo, IMapper mapper, IDaysOffRepo daysOffRepo, IWeeklyDaysOff weeklyDaysOffRepo)
        {
            this.attendenceRepo = attendenceRepo;
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
            this.daysOffRepo = daysOffRepo;
            this.weeklyDaysOffRepo = weeklyDaysOffRepo;
        }

        [Authorize(Permissions.Attendance.view)]
        [HttpGet]
        public async Task<ActionResult<PagedList<AttendanceDTO>>> GetAll([FromQuery] UserParams userParams)
        {
            var attendances = await attendenceRepo.GetAll(userParams);


            if (attendances.Count == 0)
            {
                return NotFound("Attendence list is empty");
            }

            Response.AddPaginationHeader(new PaginationHeader(attendances.CurrentPage, attendances.TotalPages, attendances.PageSize, attendances.TotalCount));


            return Ok(attendances);
        }

        [Authorize(Permissions.Attendance.create)]
        [HttpPost]
        public ActionResult Add(AttendanceDTO attendanceDTO)
        {
            DaysOff daysOff = daysOffRepo.GetByDay(attendanceDTO.Day);
            WeeklyDaysOff weeklyDays = weeklyDaysOffRepo.Get();
            if (daysOff != null)
                return BadRequest("This is an Official day off");
                
            int dayIndex = (int)attendanceDTO.Day.DayOfWeek;

            if (weeklyDays != null && weeklyDays.Days.Contains((DaysName)dayIndex))
            {
                return BadRequest("This is a weekly day off");
            }

            if (attendenceRepo.GetDayByEmpId(attendanceDTO.EmpId, attendanceDTO.Day) != null)
                return BadRequest("Attendance for this employee already added!");


            int employeeDepartureHour = (int)employeeRepo.GetById(attendanceDTO.EmpId).Departure.Hour;
            int employeeArrivalHour = (int)employeeRepo.GetById(attendanceDTO.EmpId).Arrival.Hour;


            Attendence attendence;
            if (attendanceDTO.Status == 0)
            {
                if (attendanceDTO.Arrival?.Hour < employeeArrivalHour) 
                    attendanceDTO.Arrival = employeeRepo.GetById(attendanceDTO.EmpId).Arrival;

                attendence = mapper.Map<Attendence>(attendanceDTO);
                var overtime = (int)attendanceDTO.Departure?.Hour - employeeDepartureHour;

                var latetime = (int)attendanceDTO.Arrival?.Hour - employeeArrivalHour;
                attendence.LatetimeInHours = 0;
                attendence.OvertimeInHours = 0;

                if (overtime < 0)
                {
                    attendence.LatetimeInHours += overtime * -1;
                }
                else
                {
                    attendence.OvertimeInHours += overtime;
                }
                if (latetime < 0)
                {
                    attendence.OvertimeInHours += latetime * -1;
                }
                else
                {
                    attendence.LatetimeInHours += latetime;
                }

                attendenceRepo.Add(attendence);
            }
            else
            {
                attendence = mapper.Map<Attendence>(attendanceDTO);

                attendenceRepo.Add(attendence);
            }
            attendenceRepo.Save();


            return CreatedAtAction("GetDayByEmpId", new { empId = attendence.EmpId, date = attendence.Day }, attendanceDTO);
        }

        [Authorize(Permissions.Attendance.create)]
        [HttpPost("AddExcel")]
        public ActionResult AddExcel(IFormFile file)
        {

            //convert excel to DataTable
            if (file == null || file.Length == 0)
            {
                return null;
            }
            DataTable dataTable = new DataTable();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = file.OpenReadStream())
            {
                var excelData = new List<List<object>>();
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var resultDataset = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        FilterSheet = (tableReader, sheetIndex) => true,
                        UseColumnDataType = true,

                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true,
                            //filter columns
                            FilterColumn = (rowReader, columnIndex) =>
                            {
                                return columnIndex == 0 || columnIndex == 1 || columnIndex == 2 || columnIndex == 3 || columnIndex == 4;
                            }
                        }
                    });

                    dataTable = resultDataset.Tables[0];
                }
            }

            List<DaysOff> daysOff = daysOffRepo.GetAll();
            WeeklyDaysOff weeklyDays = weeklyDaysOffRepo.Get();
            var employeesDeparture = employeeRepo.GetAll().ToDictionary(e=>e.Id, e=>e.Departure);
            var employeesArrival = employeeRepo.GetAll().ToDictionary(e => e.Id, e => e.Arrival);

            var employeesInDatabase = employeesArrival.Keys.ToList();

            //convert to attendances
            foreach (DataRow row in dataTable.Rows)
            {
                Attendence attendence = null;
                try
                {
                    attendence = new Attendence
                    {
                        EmpId = Convert.ToInt32(row["empid"]),
                        Day = DateOnly.FromDateTime(Convert.ToDateTime(row["day"])),
                        Arrival = row["Arrival"] == DBNull.Value ? null : TimeOnly.FromDateTime(Convert.ToDateTime(row["Arrival"])),
                        Departure = row["Departure"] == DBNull.Value ? null : TimeOnly.FromDateTime(Convert.ToDateTime(row["Departure"])),
                        Status = (AttendenceStatus)Enum.Parse(typeof(AttendenceStatus), row["status"].ToString())
                    };
                }
                catch(Exception e)
                {
                    return BadRequest(new { message = "Improper Excel Format"});
                }

                if (!employeesInDatabase.Contains(attendence.EmpId))
                    continue;

                if (daysOff.Any(d => d.Date == attendence.Day) || weeklyDays.Days.Contains((DaysName)(int)attendence.Day.DayOfWeek))
                {
                    continue; //skip holidays
                }



                //TODO: Add Holiday Work Logic

                //check for overtime
                var overtime = attendence.Departure.HasValue ? (int)attendence.Departure.Value.Hour - employeesDeparture[attendence.EmpId].Hour : 0;
                var latetime = attendence.Arrival.HasValue ? (int)attendence.Arrival.Value.Hour - employeesArrival[attendence.EmpId].Hour : 0;
                attendence.LatetimeInHours = 0;
                attendence.OvertimeInHours = 0;

                if (overtime < 0)
                {
                    attendence.LatetimeInHours += overtime * -1;
                }
                else
                {
                    attendence.OvertimeInHours += overtime;
                }
                if (latetime < 0)
                {
                    attendence.OvertimeInHours += latetime * -1;
                }
                else
                {
                    attendence.LatetimeInHours += latetime;
                }

                var existingAttendance = attendenceRepo.GetDayByEmpId(attendence.EmpId, attendence.Day);
                if (existingAttendance != null)
                {
                    Console.WriteLine($"--- {attendence.EmpId}, {attendence.Day} -- Updating existing attendance");

                    existingAttendance.Arrival = attendence.Arrival;
                    existingAttendance.Departure = attendence.Departure;
                    existingAttendance.Status = attendence.Status;
                    attendenceRepo.Delete(attendence.EmpId, attendence.Day);
                }

                attendenceRepo.Add(attendence);
            }

            attendenceRepo.Save();

            return Ok();
        }

        [Authorize(Permissions.Attendance.edit)]
        [HttpPut("{empId}")]
        public IActionResult Update([FromRoute] int empId, [FromQuery] DateOnly date, [FromBody] AttendanceDTO attendenceDTO)
        {
            DaysOff daysOff = daysOffRepo.GetByDay(attendenceDTO.Day);
            WeeklyDaysOff weeklyDays = weeklyDaysOffRepo.Get();
            if (daysOff != null)
                return BadRequest("This is an Official day off");

            int dayIndex = (int)attendenceDTO.Day.DayOfWeek;

            if (weeklyDays != null && weeklyDays.Days.Contains((DaysName)dayIndex))
            {
                return BadRequest("This is a weekly day off");
            }


            Attendence existingAttendence = attendenceRepo.GetDayByEmpId(empId, date);
            Employee currentEmployee = employeeRepo.GetById(empId);
            if (existingAttendence == null)
            {
                return BadRequest("Employee with specified Date Not Found");
            }
            existingAttendence.Status = (AttendenceStatus)attendenceDTO.Status;
            if (existingAttendence.Status == AttendenceStatus.Absent)
            {
                existingAttendence.Arrival = null;
                existingAttendence.Departure = null;
                existingAttendence.LatetimeInHours = null;
                existingAttendence.OvertimeInHours = null;
            }
            else
            {
                existingAttendence.Arrival = attendenceDTO.Arrival;
                existingAttendence.Departure = attendenceDTO.Departure;
                var overtime = (int)attendenceDTO.Departure?.Hour - currentEmployee.Departure.Hour;

                var latetime = (int)attendenceDTO.Arrival?.Hour - currentEmployee.Arrival.Hour;

                existingAttendence.LatetimeInHours = 0;
                existingAttendence.OvertimeInHours = 0;
                if (overtime < 0)
                {
                    existingAttendence.LatetimeInHours += overtime * -1;
                }
                else
                {
                    existingAttendence.OvertimeInHours += overtime;
                }
                if (latetime < 0)
                {
                    existingAttendence.OvertimeInHours += latetime * -1;
                }
                else
                {
                    existingAttendence.LatetimeInHours += latetime;
                }
            }
            existingAttendence.Day = attendenceDTO.Day;
            attendenceRepo.Update(empId, date, existingAttendence);
            attendenceRepo.Save();
            return Ok();
        }

        [Authorize(Permissions.Attendance.delete)]
        [HttpDelete("{empId}")]
        public IActionResult Delete([FromRoute] int empId, [FromQuery] DateOnly date)
        {
            Attendence attendence = attendenceRepo.GetDayByEmpId(empId, date);
            if (attendence == null)
            {
                return NotFound();
            }
            attendenceRepo.Delete(empId, date);
            attendenceRepo.Save();
            return NoContent();
        }


        [Authorize(Permissions.Attendance.view)]
        [HttpGet("GetEmployeeDay/{empId}")]
        public IActionResult GetDayByEmpId([FromRoute] int empId, [FromQuery] DateOnly day)
        {
            Attendence? attendence = attendenceRepo.GetDayByEmpId(empId, day);
            if (attendence == null)
            {
                return NotFound();
            }
            AttendanceDTO attendanceDTO = mapper.Map<AttendanceDTO>(attendence);

            return Ok(attendanceDTO);
        }


        [Authorize(Permissions.Attendance.view)]
        [HttpGet("{empId}")]
        public IActionResult GetAttendenceByEmpId(int empId)
        {
            List<AttendanceDTO> attendanceDTOs = new List<AttendanceDTO>();
            List<Attendence> attendences = attendenceRepo.GetAttendenceByEmpId(empId);
            if (attendences == null || attendences.Count == 0)
            {
                return NotFound("No attendance records found for the specified employee.");
            }
            foreach (var attendance in attendences)
            {
                AttendanceDTO attendanceDTO = mapper.Map<AttendanceDTO>(attendance);
                attendanceDTOs.Add(attendanceDTO);
            }
            return Ok(attendanceDTOs);
        }

        [Authorize(Permissions.Attendance.view)]
        [HttpGet("GetByPeriod")]
        public ActionResult GetByPeriod([FromQuery] Period period)
        {

            List<Attendence> attendences = attendenceRepo.GetByPeriod(period.Start, period.End);

            if (attendences.Count() == 0)
                return NotFound("There is no Attendances.");

            List<AttendanceDTO> attendanceDTOs = new List<AttendanceDTO>();

            foreach (var attendance in attendences)
            {
                AttendanceDTO attendanceDTO = mapper.Map<AttendanceDTO>(attendance);
                attendanceDTOs.Add(attendanceDTO);
            }

            return Ok(attendanceDTOs);
        }
    }

    public class Period
    {
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
    }
}
