using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> AddEmployeeAsync(EmployeeCreateDto dto);
    Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);
    Task DeleteEmployeeAsync(int id);
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<IReadOnlyList<EmployeeDto>> GetAllEmployeesAsync();
    Task<IReadOnlyList<EmployeeDto>> SearchEmployeesByNameAsync(string name);
    Task<IReadOnlyList<EmployeeDto>> FilterEmployeesByDepartmentAsync(int departmentId);
    Task<IReadOnlyList<EmployeeDto>> GetActiveEmployeesAsync();
}
