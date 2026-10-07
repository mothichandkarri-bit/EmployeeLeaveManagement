using EmployeeLeaveManagement.API.Data;
using EmployeeLeaveManagement.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .ToListAsync();

            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound("Department not found.");
            }

            return Ok(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateDepartment(
            Department department)
        {
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == department.Name);

            if (existingDepartment != null)
            {
                return BadRequest("Department already exists.");
            }

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Department created successfully.",
                departmentId = department.Id
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateDepartment(
            int id,
            Department department)
        {
            var existingDepartment = await _context.Departments
                .FindAsync(id);

            if (existingDepartment == null)
            {
                return NotFound("Department not found.");
            }

            existingDepartment.Name = department.Name;
            existingDepartment.Description = department.Description;
            existingDepartment.IsActive = department.IsActive;

            await _context.SaveChangesAsync();

            return Ok("Department updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound("Department not found.");
            }

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return Ok("Department deleted successfully.");
        }
    }
}