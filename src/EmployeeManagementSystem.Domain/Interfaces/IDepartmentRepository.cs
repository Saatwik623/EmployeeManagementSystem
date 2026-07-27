using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Domain.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<bool> NameExistsAsync(string name, int? excludeDepartmentId = null);
    Task<bool> HasEmployeesAsync(int departmentId);
}
