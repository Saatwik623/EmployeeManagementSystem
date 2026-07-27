using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Infrastructure.Data;

public static class SeedData
{
    private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static Department[] Departments => new[]
    {
        new Department { DepartmentId = 1, Name = "Engineering", Description = "Builds and maintains software products.", CreatedDate = SeedDate },
        new Department { DepartmentId = 2, Name = "Human Resources", Description = "Manages hiring, payroll and employee relations.", CreatedDate = SeedDate },
        new Department { DepartmentId = 3, Name = "Sales", Description = "Drives revenue through customer acquisition.", CreatedDate = SeedDate },
        new Department { DepartmentId = 4, Name = "Finance", Description = "Manages budgeting, accounting and financial planning.", CreatedDate = SeedDate }
    };

    public static Employee[] Employees => new[]
    {
        new Employee { EmployeeId = 1, FirstName = "Ava", LastName = "Thompson", Email = "ava.thompson@example.com", PhoneNumber = "555-0101", Salary = 95000m, HireDate = SeedDate, IsActive = true, DepartmentId = 1 },
        new Employee { EmployeeId = 2, FirstName = "Liam", LastName = "Chen", Email = "liam.chen@example.com", PhoneNumber = "555-0102", Salary = 88000m, HireDate = SeedDate, IsActive = true, DepartmentId = 1 },
        new Employee { EmployeeId = 3, FirstName = "Sophia", LastName = "Martinez", Email = "sophia.martinez@example.com", PhoneNumber = "555-0103", Salary = 72000m, HireDate = SeedDate, IsActive = true, DepartmentId = 2 },
        new Employee { EmployeeId = 4, FirstName = "Noah", LastName = "Patel", Email = "noah.patel@example.com", PhoneNumber = "555-0104", Salary = 68000m, HireDate = SeedDate, IsActive = false, DepartmentId = 3 },
        new Employee { EmployeeId = 5, FirstName = "Isabella", LastName = "Nguyen", Email = "isabella.nguyen@example.com", PhoneNumber = "555-0105", Salary = 79000m, HireDate = SeedDate, IsActive = true, DepartmentId = 4 },
        new Employee { EmployeeId = 6, FirstName = "Mason", LastName = "Brown", Email = "mason.brown@example.com", PhoneNumber = "555-0106", Salary = 91000m, HireDate = SeedDate, IsActive = true, DepartmentId = 1 }
    };
}
