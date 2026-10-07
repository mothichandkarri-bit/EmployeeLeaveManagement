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
    public class LeaveRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaveRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveRequests
        [HttpGet]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var requests = await _context.LeaveRequests.ToListAsync();

            return Ok(requests);
        }

        // GET: api/LeaveRequests/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Leave request not found.");
            }

            return Ok(request);
        }

        // POST: api/LeaveRequests
        [HttpPost]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> CreateLeaveRequest(
            LeaveRequest leaveRequest)
        {
            var balance = await _context.LeaveBalances
                .FirstOrDefaultAsync(b =>
                    b.EmployeeId == leaveRequest.EmployeeId &&
                    b.LeaveTypeId == leaveRequest.LeaveTypeId);

            if (balance == null)
            {
                return BadRequest("Leave balance not found.");
            }

            if (leaveRequest.NumberOfDays <= 0)
            {
                return BadRequest("Number of days must be greater than zero.");
            }

            if (leaveRequest.NumberOfDays > balance.RemainingDays)
            {
                return BadRequest("Insufficient leave balance.");
            }

            leaveRequest.Status = "Pending";
            leaveRequest.AppliedAt = DateTime.UtcNow;

            _context.LeaveRequests.Add(leaveRequest);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Leave request submitted successfully.",
                leaveRequestId = leaveRequest.Id
            });
        }

        // PUT: api/LeaveRequests/1/approve
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Leave request not found.");
            }

            if (request.Status != "Pending")
            {
                return BadRequest("Only pending requests can be approved.");
            }

            var balance = await _context.LeaveBalances
                .FirstOrDefaultAsync(b =>
                    b.EmployeeId == request.EmployeeId &&
                    b.LeaveTypeId == request.LeaveTypeId);

            if (balance == null)
            {
                return BadRequest("Leave balance not found.");
            }

            if (request.NumberOfDays > balance.RemainingDays)
            {
                return BadRequest("Insufficient leave balance.");
            }

            request.Status = "Approved";

            balance.UsedDays += request.NumberOfDays;

            balance.RemainingDays =
                balance.TotalDays - balance.UsedDays;

            await _context.SaveChangesAsync();

            return Ok("Leave request approved successfully.");
        }

        // PUT: api/LeaveRequests/1/reject
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Leave request not found.");
            }

            if (request.Status != "Pending")
            {
                return BadRequest("Only pending requests can be rejected.");
            }

            request.Status = "Rejected";

            await _context.SaveChangesAsync();

            return Ok("Leave request rejected successfully.");
        }
    }
}