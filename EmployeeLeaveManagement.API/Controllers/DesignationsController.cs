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
    public class DesignationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DesignationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations()
        {
            var designations = await _context.Designations
                .ToListAsync();

            return Ok(designations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id)
        {
            var designation = await _context.Designations
                .FindAsync(id);

            if (designation == null)
            {
                return NotFound("Designation not found.");
            }

            return Ok(designation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateDesignation(
            Designation designation)
        {
            var existingDesignation = await _context.Designations
                .FirstOrDefaultAsync(d => d.Name == designation.Name);

            if (existingDesignation != null)
            {
                return BadRequest("Designation already exists.");
            }

            _context.Designations.Add(designation);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Designation created successfully.",
                designationId = designation.Id
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateDesignation(
            int id,
            Designation designation)
        {
            var existingDesignation = await _context.Designations
                .FindAsync(id);

            if (existingDesignation == null)
            {
                return NotFound("Designation not found.");
            }

            existingDesignation.Name = designation.Name;
            existingDesignation.Description = designation.Description;
            existingDesignation.IsActive = designation.IsActive;

            await _context.SaveChangesAsync();

            return Ok("Designation updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var designation = await _context.Designations
                .FindAsync(id);

            if (designation == null)
            {
                return NotFound("Designation not found.");
            }

            _context.Designations.Remove(designation);

            await _context.SaveChangesAsync();

            return Ok("Designation deleted successfully.");
        }
    }
}