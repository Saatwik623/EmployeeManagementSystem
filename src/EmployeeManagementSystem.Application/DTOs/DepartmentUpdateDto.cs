using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Application.DTOs;

public class DepartmentUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }
}
