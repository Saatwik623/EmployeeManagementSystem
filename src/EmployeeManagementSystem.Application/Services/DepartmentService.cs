using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Exceptions;
using EmployeeManagementSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<DepartmentDto> AddDepartmentAsync(DepartmentCreateDto dto)
    {
        _logger.LogInformation("Adding department {Name}", dto.Name);

        if (await _departmentRepository.NameExistsAsync(dto.Name))
        {
            throw new ValidationException($"A department named '{dto.Name}' already exists.");
        }

        var department = new Department
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedDate = DateTime.UtcNow
        };

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} added successfully", department.DepartmentId);

        return MapToDto(department);
    }

    public async Task<DepartmentDto> UpdateDepartmentAsync(int id, DepartmentUpdateDto dto)
    {
        _logger.LogInformation("Updating department {DepartmentId}", id);

        var department = await _departmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);

        if (await _departmentRepository.NameExistsAsync(dto.Name, id))
        {
            throw new ValidationException($"A department named '{dto.Name}' already exists.");
        }

        department.Name = dto.Name;
        department.Description = dto.Description;

        _departmentRepository.Update(department);
        await _departmentRepository.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} updated successfully", id);

        return MapToDto(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        _logger.LogInformation("Deleting department {DepartmentId}", id);

        var department = await _departmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);

        if (await _departmentRepository.HasEmployeesAsync(id))
        {
            throw new ValidationException("Cannot delete a department that still has employees assigned to it.");
        }

        _departmentRepository.Remove(department);
        await _departmentRepository.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} deleted successfully", id);
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);

        return MapToDto(department);
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(MapToDto).ToList();
    }

    private static DepartmentDto MapToDto(Department department) => new()
    {
        DepartmentId = department.DepartmentId,
        Name = department.Name,
        Description = department.Description,
        CreatedDate = department.CreatedDate,
        EmployeeCount = department.Employees?.Count ?? 0
    };
}
