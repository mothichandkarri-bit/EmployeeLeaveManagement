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
    public class LeaveBalancesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaveBalancesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveBalances
        [HttpGet]
        public async Task<IActionResult> GetLeaveBalances()
        {
            var balances = await _context.LeaveBalances.ToListAsync();

            return Ok(balances);
        }

        // GET: api/LeaveBalances/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveBalance(int id)
        {
            var balance = await _context.LeaveBalances.FindAsync(id);

            if (balance == null)
            {
                return NotFound("Leave balance not found.");
            }

            return Ok(balance);
        }

        // POST: api/LeaveBalances
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateLeaveBalance(
            LeaveBalance leaveBalance)
        {
            leaveBalance.RemainingDays =
                leaveBalance.TotalDays - leaveBalance.UsedDays;

            _context.LeaveBalances.Add(leaveBalance);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Leave balance created successfully.",
                leaveBalanceId = leaveBalance.Id
            });
        }

        // PUT: api/LeaveBalances/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateLeaveBalance(
            int id,
            LeaveBalance leaveBalance)
        {
            var existingBalance =
                await _context.LeaveBalances.FindAsync(id);

            if (existingBalance == null)
            {
                return NotFound("Leave balance not found.");
            }

            existingBalance.EmployeeId = leaveBalance.EmployeeId;
            existingBalance.LeaveTypeId = leaveBalance.LeaveTypeId;
            existingBalance.TotalDays = leaveBalance.TotalDays;
            existingBalance.UsedDays = leaveBalance.UsedDays;

            existingBalance.RemainingDays =
                existingBalance.TotalDays -
                existingBalance.UsedDays;

            await _context.SaveChangesAsync();

            return Ok("Leave balance updated successfully.");
        }

        // DELETE: api/LeaveBalances/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeaveBalance(int id)
        {
            var balance =
                await _context.LeaveBalances.FindAsync(id);

            if (balance == null)
            {
                return NotFound("Leave balance not found.");
            }

            _context.LeaveBalances.Remove(balance);

            await _context.SaveChangesAsync();

            return Ok("Leave balance deleted successfully.");
        }
    }
}