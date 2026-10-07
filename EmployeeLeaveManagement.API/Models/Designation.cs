namespace EmployeeLeaveManagement.API.Models
{
    public class Designation
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}