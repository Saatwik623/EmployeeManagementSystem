namespace EmployeeManagementSystem.Infrastructure.Data;

public class EmployeeDetailsView
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}
