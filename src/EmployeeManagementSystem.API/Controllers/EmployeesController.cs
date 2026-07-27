using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.API.Controllers;

/// <summary>
/// Manages employee records: CRUD operations, search and filtering.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>Gets all employees.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    /// <summary>Gets a single employee by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    /// <summary>Searches employees whose first or last name contains the given term.</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> Search([FromQuery] string name)
    {
        var employees = await _employeeService.SearchEmployeesByNameAsync(name);
        return Ok(employees);
    }

    /// <summary>Filters employees belonging to the given department.</summary>
    [HttpGet("department/{departmentId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetByDepartment(int departmentId)
    {
        var employees = await _employeeService.FilterEmployeesByDepartmentAsync(departmentId);
        return Ok(employees);
    }

    /// <summary>Gets all currently active employees.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetActive()
    {
        var employees = await _employeeService.GetActiveEmployeesAsync();
        return Ok(employees);
    }

    /// <summary>Creates a new employee.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] EmployeeCreateDto dto)
    {
        var created = await _employeeService.AddEmployeeAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
    }

    /// <summary>Updates an existing employee.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] EmployeeUpdateDto dto)
    {
        var updated = await _employeeService.UpdateEmployeeAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Deletes an employee.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}
