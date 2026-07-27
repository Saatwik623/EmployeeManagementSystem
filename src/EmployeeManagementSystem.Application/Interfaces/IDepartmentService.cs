using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDto> AddDepartmentAsync(DepartmentCreateDto dto);
    Task<DepartmentDto> UpdateDepartmentAsync(int id, DepartmentUpdateDto dto);
    Task DeleteDepartmentAsync(int id);
    Task<DepartmentDto> GetDepartmentByIdAsync(int id);
    Task<IReadOnlyList<DepartmentDto>> GetAllDepartmentsAsync();
}
