using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IMapper mapper;

        public DepartmentController(IDepartmentRepo departmentRepo, IEmployeeRepo employeeRepo, IMapper mapper)
        {
            this.departmentRepo = departmentRepo;
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Department> departments = departmentRepo.GetAll();
            if (departments.Count == 0)
            {
                return Unauthorized();
            }

            List<DepartmentDTO> departmentDTOs = mapper.Map<List<DepartmentDTO>>(departments);
            return Ok(departmentDTOs);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            Department department = departmentRepo.GetByName(name);
            if (department == null)
            {
                return NotFound();
            }

            DepartmentDTO departmentDTO = mapper.Map<DepartmentDTO>(department);
            return Ok(departmentDTO);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DepartmentDTO departmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Department department = mapper.Map<Department>(departmentDTO);
            departmentRepo.Create(department);
            return Ok("Department created successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DepartmentDTO departmentDTO)
        {
            Department department = departmentRepo.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = departmentDTO.Name;
            departmentRepo.Update(department);
            return Ok("Department updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Department department = departmentRepo.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            var employees = employeeRepo.GetByDepartmentId(id);
            foreach (var employee in employees)
            {
                employeeRepo.Delete(employee.Id);
            }

            departmentRepo.Delete(department);
            return Ok("Department and associated employees deleted successfully");
        }
    }
}
