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
    public class LeaveTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaveTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveTypes
        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = await _context.LeaveTypes.ToListAsync();

            return Ok(leaveTypes);
        }

        // GET: api/LeaveTypes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound("Leave type not found.");
            }

            return Ok(leaveType);
        }

        // POST: api/LeaveTypes
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateLeaveType(LeaveType leaveType)
        {
            _context.LeaveTypes.Add(leaveType);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Leave type created successfully.",
                leaveTypeId = leaveType.Id
            });
        }

        // PUT: api/LeaveTypes/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateLeaveType(
            int id,
            LeaveType leaveType)
        {
            var existingLeaveType = await _context.LeaveTypes.FindAsync(id);

            if (existingLeaveType == null)
            {
                return NotFound("Leave type not found.");
            }

            existingLeaveType.Name = leaveType.Name;
            existingLeaveType.TotalDays = leaveType.TotalDays;
            existingLeaveType.IsActive = leaveType.IsActive;

            await _context.SaveChangesAsync();

            return Ok("Leave type updated successfully.");
        }

        // DELETE: api/LeaveTypes/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound("Leave type not found.");
            }

            _context.LeaveTypes.Remove(leaveType);

            await _context.SaveChangesAsync();

            return Ok("Leave type deleted successfully.");
        }
    }
}