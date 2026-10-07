using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.API.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [Range(1, int.MaxValue)]
        public int DesignationId { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        public DateTime JoiningDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}