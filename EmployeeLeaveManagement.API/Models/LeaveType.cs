namespace EmployeeLeaveManagement.API.Models
{
    public class LeaveType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TotalDays { get; set; }

        public bool IsActive { get; set; } = true;
    }
}