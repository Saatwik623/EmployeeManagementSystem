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

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
    private readonly Mock<ILogger<DepartmentService>> _loggerMock = new();
    private readonly DepartmentService _sut;

    public DepartmentServiceTests()
    {
        _sut = new DepartmentService(_departmentRepositoryMock.Object, _loggerMock.Object);
    }

    private static Department CreateDepartment(int id = 1, string name = "Engineering") => new()
    {
        DepartmentId = id,
        Name = name,
        Description = "Builds and maintains software products.",
        CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        Employees = new List<Employee>()
    };

    [Fact]
    public async Task AddDepartmentAsync_ValidInput_ReturnsCreatedDepartment()
    {
        var dto = new DepartmentCreateDto { Name = "Engineering", Description = "Builds software." };
        _departmentRepositoryMock.Setup(r => r.NameExistsAsync(dto.Name, null)).ReturnsAsync(false);

        var result = await _sut.AddDepartmentAsync(dto);

        result.Name.Should().Be("Engineering");
        _departmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Department>()), Times.Once);
        _departmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddDepartmentAsync_DuplicateName_ThrowsValidationException()
    {
        var dto = new DepartmentCreateDto { Name = "Engineering" };
        _departmentRepositoryMock.Setup(r => r.NameExistsAsync(dto.Name, null)).ReturnsAsync(true);

        var act = () => _sut.AddDepartmentAsync(dto);

        await act.Should().ThrowAsync<ValidationException>();
        _departmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_NotFound_ThrowsNotFoundException()
    {
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);
        var dto = new DepartmentUpdateDto { Name = "Engineering" };

        var act = () => _sut.UpdateDepartmentAsync(1, dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateDepartmentAsync_DuplicateName_ThrowsValidationException()
    {
        var department = CreateDepartment();
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
        _departmentRepositoryMock.Setup(r => r.NameExistsAsync("Sales", 1)).ReturnsAsync(true);
        var dto = new DepartmentUpdateDto { Name = "Sales" };

        var act = () => _sut.UpdateDepartmentAsync(1, dto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ValidInput_ReturnsUpdatedDepartment()
    {
        var department = CreateDepartment();
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
        _departmentRepositoryMock.Setup(r => r.NameExistsAsync("Product Engineering", 1)).ReturnsAsync(false);
        var dto = new DepartmentUpdateDto { Name = "Product Engineering", Description = "Updated description." };

        var result = await _sut.UpdateDepartmentAsync(1, dto);

        result.Name.Should().Be("Product Engineering");
        result.Description.Should().Be("Updated description.");
        _departmentRepositoryMock.Verify(r => r.Update(department), Times.Once);
        _departmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_NotFound_ThrowsNotFoundException()
    {
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);

        var act = () => _sut.DeleteDepartmentAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteDepartmentAsync_HasEmployees_ThrowsValidationException()
    {
        var department = CreateDepartment();
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
        _departmentRepositoryMock.Setup(r => r.HasEmployeesAsync(1)).ReturnsAsync(true);

        var act = () => _sut.DeleteDepartmentAsync(1);

        await act.Should().ThrowAsync<ValidationException>();
        _departmentRepositoryMock.Verify(r => r.Remove(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ValidInput_RemovesDepartment()
    {
        var department = CreateDepartment();
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
        _departmentRepositoryMock.Setup(r => r.HasEmployeesAsync(1)).ReturnsAsync(false);

        await _sut.DeleteDepartmentAsync(1);

        _departmentRepositoryMock.Verify(r => r.Remove(department), Times.Once);
        _departmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_NotFound_ThrowsNotFoundException()
    {
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);

        var act = () => _sut.GetDepartmentByIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_Found_ReturnsDto()
    {
        _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateDepartment());

        var result = await _sut.GetDepartmentByIdAsync(1);

        result.DepartmentId.Should().Be(1);
        result.Name.Should().Be("Engineering");
    }

    [Fact]
    public async Task GetAllDepartmentsAsync_ReturnsAllMapped()
    {
        var departments = new List<Department> { CreateDepartment(1, "Engineering"), CreateDepartment(2, "Sales") };
        _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

        var result = await _sut.GetAllDepartmentsAsync();

        result.Should().HaveCount(2);
    }
}
