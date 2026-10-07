using EmployeeLeaveManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<Attendance> Attendances { get; set; }
    }
}