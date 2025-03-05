using Microsoft.Data.SqlClient;
using System.Data;
using WebAPI.Constants;
using WebAPI.Interfaces;
using WebAPI.Models;
using Microsoft.Extensions.Configuration;

namespace WebAPI.ProjectProcessing
{
    public class PayrollCalculator
    {
        const int TOTAL_WORKING_DAYS_PER_MONTH = 30;
        private int[] EmpIds;
        private DateOnly PayslipStartDate;
        private DateOnly PayslipEndDate;

        private IEmployeeRepo EmployeeRepo;
        private IAttendence AttendenceRepo;
        private ICommission CommissionRepo;
        private IDeduction DeductionRepo;
        private readonly IConfiguration _configuration;

        private decimal BaseSalary { get; set; }
        private decimal SalaryPerDay { get; set; }
        private decimal SalaryPerHour { get; set; }
        private int WorkingHours { get; set; }
        private Employee currentEmployee;

        // payslip data
        private int AttendanceDays;
        private int OvertimeHours;
        private int AbsenceDays;
        private int LatenessHours;


        private decimal LatenessHoursPay;
        private decimal TotalDeductions;
        private decimal TotalAdditional;
        private decimal AbsenceDaysPay;
        private decimal OvertimePay;
        private decimal NetSalary;


        public PayrollCalculator(IConfiguration configuration, IEmployeeRepo EmployeeRepo, IAttendence AttendenceRepo, ICommission CommissionRepo, IDeduction DeductionRepo)
        {
            _configuration = configuration;
            this.EmployeeRepo = EmployeeRepo;
            this.AttendenceRepo = AttendenceRepo;
            this.CommissionRepo = CommissionRepo;
            this.DeductionRepo = DeductionRepo;
        }


        // set employee and payslip required data
        //public void SetPayrollData(int[] EmpIds, DateOnly PayslipStartDate, DateOnly PayslipEndDate)
        public void SetPayrollData(DateOnly PayslipStartDate, DateOnly PayslipEndDate)
        {
            //this.EmpIds = EmpIds;
            this.PayslipStartDate = PayslipStartDate;
            this.PayslipEndDate = PayslipEndDate;
        }

        //public void GetEmployeeData(int EmpId)
        public void GetEmployeeData(Employee Emp)
        {
            // TODO: load employee from database
            //currentEmployee = EmployeeRepo.GetById(EmpId);
            currentEmployee = Emp;
            // TODO: GetEmployeeSalaryData
            BaseSalary = currentEmployee.BaseSalary;
            // TODO: calculate employee working hours
            WorkingHours = CalculateWorkingHours();
            // TODO: calculate employee salary per day
            SalaryPerDay = CalculateSalaryPerDay();
            // TODO: calculate employee salary per hour
            SalaryPerHour = CalculateSalaryPerHour();
            // TODO: calculate employee Attendance days
            AttendanceDays = CalculateAttendanceDays();
            // TODO: calculate employee Absence days
            AbsenceDays = CalculateAbsenceDays();
            // TODO: calculate overtime hours
            OvertimeHours = CalculateOvertimeHours();
            //TODO: calculate latetime hours
            LatenessHours = CalculateLatenessHours();
        }

        //// get employee salary data
        //public void GetEmployeeSalary()
        //{
        //    // TODO: get salary from employee repo
        //    // TODO: set base salary here
        //}

        private decimal CalculateSalaryPerDay() { return Math.Round(BaseSalary / TOTAL_WORKING_DAYS_PER_MONTH, 2); }

        private decimal CalculateSalaryPerHour() { return Math.Round(SalaryPerDay / WorkingHours, 2); }

        private int CalculateWorkingHours() { return (currentEmployee.Departure.Hour) - currentEmployee.Arrival.Hour; }

        private int CalculateAttendanceDays() { 
            //return AttendenceRepo.GetAttendenceByEmpId(currentEmployee.Id)
            //    .Where(att => att.Status == AttendenceStatus.Present)
            //    .ToList().Count;

            return AttendenceRepo.GetByPeriod(PayslipStartDate, PayslipEndDate)
                .Where(att => att.EmpId == currentEmployee.Id && att.Status == AttendenceStatus.Present)
                .ToList().Count;
        }

        private int CalculateAbsenceDays() {
            //return AttendenceRepo.GetAttendenceByEmpId(currentEmployee.Id)
            //    .Where(att => att.Status == AttendenceStatus.Absent)
            //    .ToList().Count;
            return AttendenceRepo.GetByPeriod(PayslipStartDate, PayslipEndDate)
                .Where(att => att.EmpId == currentEmployee.Id && att.Status == AttendenceStatus.Absent)
                .ToList().Count;
        }

        private int CalculateOvertimeHours() {
            List<int> overtime = new List<int>();

            //overtime = (List<int>)AttendenceRepo.GetAttendenceByEmpId(currentEmployee.Id)
            //    .Where(att => att.Status == AttendenceStatus.Present)
            //    .ToList().Select(att => att.OvertimeInHours);
            overtime = AttendenceRepo.GetByPeriod(PayslipStartDate, PayslipEndDate)
                .Where(att => att.EmpId == currentEmployee.Id && att.Status == AttendenceStatus.Present)
                .Select(att => att.OvertimeInHours ?? 0)
                .ToList();
            

            return overtime.Sum();
        }

        private int CalculateLatenessHours() {
            List<int> latetime = new List<int>();
            //latetime = (List<int>)AttendenceRepo.GetAttendenceByEmpId(currentEmployee.Id)
            //    .Where(att => att.Status == AttendenceStatus.Present)
            //    .ToList().Select(att => att.LatetimeInHours);
            latetime = AttendenceRepo.GetByPeriod(PayslipStartDate, PayslipEndDate)
              .Where(att => att.EmpId == currentEmployee.Id && att.Status == AttendenceStatus.Present)
              .Select(att => att.LatetimeInHours ?? 0)
              .ToList();
            return latetime.Sum();
        }

        public List<Payslip> generatePayslips(DateOnly startDate, DateOnly endDate)
        {
            List<Payslip> result = new List<Payslip>();

            // Define the stored procedure name
            string storedProcedure = "[dbo].[GetEmployeePayslip]";

            try
            {
                string connectionString = _configuration.GetConnectionString("cs");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@st_date", startDate);
                        cmd.Parameters.AddWithValue("@end_date", endDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database result to the Payslip object
                                Payslip payslip = new Payslip
                                {
                                    FullName = reader["FullName"].ToString(),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    BaseSalary = Convert.ToDecimal(reader["BaseSalary"]),
                                    AttendanceDays = Convert.ToInt32(reader["AttendanceDays"]),
                                    AbsenceDays = Convert.ToInt32(reader["AbsentDays"]),
                                    OvertimeHours = Convert.ToInt32(reader["TotalOvertimeHours"]),
                                    LatenessHours = Convert.ToInt32(reader["TotalLateHours"]),
                                    TotalAdditional = Convert.ToDecimal(reader["AdditionalPay"]),
                                    TotalDeduction = Convert.ToDecimal(reader["DeductionPay"]),
                                    NetSalary = Convert.ToDecimal(reader["CalculatedSalary"])
                                };

                                result.Add(payslip);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error: {ex.Message}");
                // Optionally, rethrow or handle the exception based on your requirements
            }

            return result;
        }


    //public decimal CalculateInHours(int Hours)
    //{
    //    decimal Money;
    //    Money = Hours * SalaryPerHour;
    //    return Money;
    //}


    //public decimal CalculateInAmount(decimal Amount)
    //{
    //    double ToHours = ConvertAmountToHours(Amount);
    //    decimal Money;
    //    Money = (decimal)ToHours * SalaryPerHour;
    //    return Money;
    //}


    //public decimal CalculateAbsentDays(int days)
    //{
    //    decimal TotalAbsentMoney;
    //    TotalAbsentMoney = SalaryPerDay * days;
    //    return TotalAbsentMoney;
    //}



}
}