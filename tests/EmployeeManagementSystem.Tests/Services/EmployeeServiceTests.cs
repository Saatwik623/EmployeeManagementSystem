using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Services;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Exceptions;
using EmployeeManagementSystem.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EmployeeManagementSystem.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
    private readonly Mock<ILogger<EmployeeService>> _loggerMock = new();
    private readonly EmployeeService _sut;

    public EmployeeServiceTests()
    {
        _sut = new EmployeeService(_employeeRepositoryMock.Object, _departmentRepositoryMock.Object, _loggerMock.Object);
    }

    private static Employee CreateEmployee(int id = 1, int departmentId = 1, bool isActive = true, string email = "jane.doe@example.com") => new()
    {
        EmployeeId = id,
        FirstName = "Jane",
        LastName = "Doe",
        Email = email,
        PhoneNumber = "555-0000",
        Salary = 50000m,
        HireDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsActive = isActive,
        DepartmentId = departmentId,
        Department = new Department { DepartmentId = departmentId, Name = "Engineering" }
    };

    [Fact]
    public async Task AddEmployeeAsync_ValidInput_ReturnsCreatedEmployee()
    {
        var dto = new EmployeeCreateDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Salary = 50000m,
            DepartmentId = 1
        };

        _departmentRepositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email, null)).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(r => r.GetByIdWithDepartmentAsync(It.IsAny<int>()))
            .ReturnsAsync(CreateEmployee());

        var result = await _sut.AddEmployeeAsync(dto);

        result.Should().NotBeNull();
        result.Email.Should().Be(dto.Email);
        result.DepartmentName.Should().Be("Engineering");
        _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        _employeeRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddEmployeeAsync_DepartmentDoesNotExist_ThrowsValidationException()
    {
        var dto = new EmployeeCreateDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DepartmentId = 99 };
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var act = () => _sut.AddEmployeeAsync(dto);

        await act.Should().ThrowAsync<ValidationException>();
        _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task AddEmployeeAsync_DuplicateEmail_ThrowsValidationException()
    {
        var dto = new EmployeeCreateDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DepartmentId = 1 };
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email, null)).ReturnsAsync(true);

        var act = () => _sut.AddEmployeeAsync(dto);

        await act.Should().ThrowAsync<ValidationException>();
        _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_EmployeeNotFound_ThrowsNotFoundException()
    {
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee?)null);
        var dto = new EmployeeUpdateDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DepartmentId = 1 };

        var act = () => _sut.UpdateEmployeeAsync(1, dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateEmployeeAsync_DepartmentDoesNotExist_ThrowsValidationException()
    {
        var employee = CreateEmployee();
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);
        var dto = new EmployeeUpdateDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DepartmentId = 99 };

        var act = () => _sut.UpdateEmployeeAsync(1, dto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateEmployeeAsync_DuplicateEmail_ThrowsValidationException()
    {
        var employee = CreateEmployee();
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(r => r.EmailExistsAsync("taken@example.com", 1)).ReturnsAsync(true);
        var dto = new EmployeeUpdateDto { FirstName = "Jane", LastName = "Doe", Email = "taken@example.com", DepartmentId = 1 };

        var act = () => _sut.UpdateEmployeeAsync(1, dto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ValidInput_ReturnsUpdatedEmployee()
    {
        var employee = CreateEmployee();
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(r => r.EmailExistsAsync("jane.updated@example.com", 1)).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(r => r.GetByIdWithDepartmentAsync(1))
            .ReturnsAsync(CreateEmployee(email: "jane.updated@example.com"));

        var dto = new EmployeeUpdateDto { FirstName = "Jane", LastName = "Doe", Email = "jane.updated@example.com", Salary = 60000m, DepartmentId = 1, IsActive = true };

        var result = await _sut.UpdateEmployeeAsync(1, dto);

        result.Email.Should().Be("jane.updated@example.com");
        _employeeRepositoryMock.Verify(r => r.Update(It.IsAny<Employee>()), Times.Once);
        _employeeRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_EmployeeNotFound_ThrowsNotFoundException()
    {
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

        var act = () => _sut.DeleteEmployeeAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ValidId_RemovesEmployee()
    {
        var employee = CreateEmployee();
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

        await _sut.DeleteEmployeeAsync(1);

        _employeeRepositoryMock.Verify(r => r.Remove(employee), Times.Once);
        _employeeRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_NotFound_ThrowsNotFoundException()
    {
        _employeeRepositoryMock.Setup(r => r.GetByIdWithDepartmentAsync(1)).ReturnsAsync((Employee?)null);

        var act = () => _sut.GetEmployeeByIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_Found_ReturnsDto()
    {
        _employeeRepositoryMock.Setup(r => r.GetByIdWithDepartmentAsync(1)).ReturnsAsync(CreateEmployee());

        var result = await _sut.GetEmployeeByIdAsync(1);

        result.EmployeeId.Should().Be(1);
        result.FullName.Should().Be("Jane Doe");
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ReturnsAllMapped()
    {
        var employees = new List<Employee> { CreateEmployee(1), CreateEmployee(2) };
        _employeeRepositoryMock.Setup(r => r.GetAllWithDepartmentAsync()).ReturnsAsync(employees);

        var result = await _sut.GetAllEmployeesAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchEmployeesByNameAsync_EmptyTerm_ThrowsValidationException()
    {
        var act = () => _sut.SearchEmployeesByNameAsync("   ");

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task SearchEmployeesByNameAsync_ValidTerm_ReturnsMatches()
    {
        var employees = new List<Employee> { CreateEmployee() };
        _employeeRepositoryMock.Setup(r => r.SearchByNameAsync("Jane")).ReturnsAsync(employees);

        var result = await _sut.SearchEmployeesByNameAsync("Jane");

        result.Should().ContainSingle();
    }

    [Fact]
    public async Task FilterEmployeesByDepartmentAsync_DepartmentNotFound_ThrowsNotFoundException()
    {
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var act = () => _sut.FilterEmployeesByDepartmentAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task FilterEmployeesByDepartmentAsync_ValidDepartment_ReturnsEmployees()
    {
        _departmentRepositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _employeeRepositoryMock.Setup(r => r.GetByDepartmentAsync(1)).ReturnsAsync(new List<Employee> { CreateEmployee() });

        var result = await _sut.FilterEmployeesByDepartmentAsync(1);

        result.Should().ContainSingle();
    }

    [Fact]
    public async Task GetActiveEmployeesAsync_ReturnsOnlyActive()
    {
        _employeeRepositoryMock.Setup(r => r.GetActiveEmployeesAsync())
            .ReturnsAsync(new List<Employee> { CreateEmployee(isActive: true) });

        var result = await _sut.GetActiveEmployeesAsync();

        result.Should().OnlyContain(e => e.IsActive);
    }
}
