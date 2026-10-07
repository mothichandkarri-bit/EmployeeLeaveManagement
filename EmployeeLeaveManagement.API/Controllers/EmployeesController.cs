using EmployeeLeaveManagement.API.Data;
using EmployeeLeaveManagement.API.DTOs;
using EmployeeLeaveManagement.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,

                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department!.Name,

                    DesignationId = e.DesignationId,
                    DesignationName = e.Designation!.Name,

                    UserId = e.UserId,

                    JoiningDate = e.JoiningDate,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,

                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department!.Name,

                    DesignationId = e.DesignationId,
                    DesignationName = e.Designation!.Name,

                    UserId = e.UserId,

                    JoiningDate = e.JoiningDate,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateEmployee(
            CreateEmployeeDto dto)
        {
            var existingEmail = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            if (existingEmail != null)
            {
                return BadRequest("Employee email already exists.");
            }

            var existingEmployeeCode = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode == dto.EmployeeCode);

            if (existingEmployeeCode != null)
            {
                return BadRequest("Employee code already exists.");
            }

            var department = await _context.Departments
                .FindAsync(dto.DepartmentId);

            if (department == null)
            {
                return BadRequest("Department not found.");
            }

            var designation = await _context.Designations
                .FindAsync(dto.DesignationId);

            if (designation == null)
            {
                return BadRequest("Designation not found.");
            }

            var user = await _context.Users
                .FindAsync(dto.UserId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var existingEmployeeUser = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == dto.UserId);

            if (existingEmployeeUser != null)
            {
                return BadRequest("This user is already linked to an employee.");
            }

            var employee = new Employee
            {
                EmployeeCode = dto.EmployeeCode,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                DepartmentId = dto.DepartmentId,
                DesignationId = dto.DesignationId,
                UserId = dto.UserId,
                JoiningDate = dto.JoiningDate,
                IsActive = dto.IsActive
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Employee created successfully.",
                employeeId = employee.Id
            });
        }

        // PUT: api/Employees/6
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateEmployee(
            int id,
            UpdateEmployeeDto dto)
        {
            var existingEmployee = await _context.Employees
                .FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }

            var existingEmail = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.Email == dto.Email &&
                    e.Id != id);

            if (existingEmail != null)
            {
                return BadRequest("Employee email already exists.");
            }

            var existingEmployeeCode = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.EmployeeCode == dto.EmployeeCode &&
                    e.Id != id);

            if (existingEmployeeCode != null)
            {
                return BadRequest("Employee code already exists.");
            }

            var department = await _context.Departments
                .FindAsync(dto.DepartmentId);

            if (department == null)
            {
                return BadRequest("Department not found.");
            }

            var designation = await _context.Designations
                .FindAsync(dto.DesignationId);

            if (designation == null)
            {
                return BadRequest("Designation not found.");
            }

            var user = await _context.Users
                .FindAsync(dto.UserId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var existingEmployeeUser = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.UserId == dto.UserId &&
                    e.Id != id);

            if (existingEmployeeUser != null)
            {
                return BadRequest("This user is already linked to another employee.");
            }

            existingEmployee.EmployeeCode = dto.EmployeeCode;
            existingEmployee.FullName = dto.FullName;
            existingEmployee.Email = dto.Email;
            existingEmployee.Phone = dto.Phone;
            existingEmployee.DepartmentId = dto.DepartmentId;
            existingEmployee.DesignationId = dto.DesignationId;
            existingEmployee.UserId = dto.UserId;
            existingEmployee.JoiningDate = dto.JoiningDate;
            existingEmployee.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return Ok("Employee updated successfully.");
        }

        // DELETE: api/Employees/6
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees
                .FindAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok("Employee deleted successfully.");
        }
    }
}