using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByIdWithDepartmentAsync(int id);
    Task<IReadOnlyList<Employee>> GetAllWithDepartmentAsync();
    Task<IReadOnlyList<Employee>> SearchByNameAsync(string name);
    Task<IReadOnlyList<Employee>> GetByDepartmentAsync(int departmentId);
    Task<IReadOnlyList<Employee>> GetActiveEmployeesAsync();
    Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null);
}
