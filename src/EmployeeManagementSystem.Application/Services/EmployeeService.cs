using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Exceptions;
using EmployeeManagementSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<EmployeeDto> AddEmployeeAsync(EmployeeCreateDto dto)
    {
        _logger.LogInformation("Adding employee with email {Email}", dto.Email);

        if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
        {
            throw new ValidationException($"Department with id '{dto.DepartmentId}' does not exist.");
        }

        if (await _employeeRepository.EmailExistsAsync(dto.Email))
        {
            throw new ValidationException($"An employee with email '{dto.Email}' already exists.");
        }

        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Salary = dto.Salary,
            HireDate = dto.HireDate,
            IsActive = dto.IsActive,
            DepartmentId = dto.DepartmentId
        };

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        _logger.LogInformation("Employee {EmployeeId} added successfully", employee.EmployeeId);

        var created = await _employeeRepository.GetByIdWithDepartmentAsync(employee.EmployeeId);
        return MapToDto(created!);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
    {
        _logger.LogInformation("Updating employee {EmployeeId}", id);

        var employee = await _employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
        {
            throw new ValidationException($"Department with id '{dto.DepartmentId}' does not exist.");
        }

        if (await _employeeRepository.EmailExistsAsync(dto.Email, id))
        {
            throw new ValidationException($"An employee with email '{dto.Email}' already exists.");
        }

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.PhoneNumber = dto.PhoneNumber;
        employee.Salary = dto.Salary;
        employee.IsActive = dto.IsActive;
        employee.DepartmentId = dto.DepartmentId;

        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();

        _logger.LogInformation("Employee {EmployeeId} updated successfully", id);

        var updated = await _employeeRepository.GetByIdWithDepartmentAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        _logger.LogInformation("Deleting employee {EmployeeId}", id);

        var employee = await _employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        _employeeRepository.Remove(employee);
        await _employeeRepository.SaveChangesAsync();

        _logger.LogInformation("Employee {EmployeeId} deleted successfully", id);
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdWithDepartmentAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        return MapToDto(employee);
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllWithDepartmentAsync();
        return employees.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<EmployeeDto>> SearchEmployeesByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Search term cannot be empty.");
        }

        var employees = await _employeeRepository.SearchByNameAsync(name);
        return employees.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<EmployeeDto>> FilterEmployeesByDepartmentAsync(int departmentId)
    {
        if (!await _departmentRepository.ExistsAsync(departmentId))
        {
            throw new NotFoundException(nameof(Department), departmentId);
        }

        var employees = await _employeeRepository.GetByDepartmentAsync(departmentId);
        return employees.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetActiveEmployeesAsync()
    {
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        return employees.Select(MapToDto).ToList();
    }

    private static EmployeeDto MapToDto(Employee employee) => new()
    {
        EmployeeId = employee.EmployeeId,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email,
        PhoneNumber = employee.PhoneNumber,
        Salary = employee.Salary,
        HireDate = employee.HireDate,
        IsActive = employee.IsActive,
        DepartmentId = employee.DepartmentId,
        DepartmentName = employee.Department?.Name
    };
}
