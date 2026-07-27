using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Application.DTOs;

public class EmployeeUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public int DepartmentId { get; set; }
}
