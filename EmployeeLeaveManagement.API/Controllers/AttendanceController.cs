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
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance()
        {
            var attendance = await _context.Attendances
                .ToListAsync();

            return Ok(attendance);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendanceById(int id)
        {
            var attendance = await _context.Attendances
                .FindAsync(id);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            return Ok(attendance);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAttendance(int employeeId)
        {
            var attendance = await _context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();

            return Ok(attendance);
        }

        [HttpPost("checkin")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> CheckIn(int employeeId)
        {
            var today = DateTime.Today;

            var existingAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == today);

            if (existingAttendance != null)
            {
                return BadRequest("Employee has already checked in today.");
            }

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                AttendanceDate = today,
                CheckInTime = DateTime.Now,
                Status = "Present"
            };

            _context.Attendances.Add(attendance);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Check-in successful.",
                attendanceId = attendance.Id,
                checkInTime = attendance.CheckInTime
            });
        }

        [HttpPut("checkout")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> CheckOut(int employeeId)
        {
            var today = DateTime.Today;

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == today);

            if (attendance == null)
            {
                return NotFound("Employee has not checked in today.");
            }

            if (attendance.CheckOutTime != null)
            {
                return BadRequest("Employee has already checked out today.");
            }

            attendance.CheckOutTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Check-out successful.",
                checkOutTime = attendance.CheckOutTime
            });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateAttendanceStatus(
            int id,
            string status)
        {
            var attendance = await _context.Attendances
                .FindAsync(id);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            attendance.Status = status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Attendance status updated successfully.",
                attendanceId = attendance.Id,
                status = attendance.Status
            });
        }

        [HttpPut("{id}/remarks")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateAttendanceRemarks(
            int id,
            string remarks)
        {
            var attendance = await _context.Attendances
                .FindAsync(id);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            attendance.Remarks = remarks;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Attendance remarks updated successfully.",
                attendanceId = attendance.Id,
                remarks = attendance.Remarks
            });
        }
    }
}