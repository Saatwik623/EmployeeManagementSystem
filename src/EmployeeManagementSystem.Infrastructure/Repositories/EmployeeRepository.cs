using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;
using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByIdWithDepartmentAsync(int id) =>
        await DbSet.Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

    public async Task<IReadOnlyList<Employee>> GetAllWithDepartmentAsync() =>
        await DbSet.Include(e => e.Department)
            .OrderBy(e => e.LastName)
            .ToListAsync();

    public async Task<IReadOnlyList<Employee>> SearchByNameAsync(string name) =>
        await DbSet.Include(e => e.Department)
            .Where(e => EF.Functions.Like(e.FirstName, $"%{name}%") || EF.Functions.Like(e.LastName, $"%{name}%"))
            .OrderBy(e => e.LastName)
            .ToListAsync();

    public async Task<IReadOnlyList<Employee>> GetByDepartmentAsync(int departmentId) =>
        await DbSet.Include(e => e.Department)
            .Where(e => e.DepartmentId == departmentId)
            .OrderBy(e => e.LastName)
            .ToListAsync();

    public async Task<IReadOnlyList<Employee>> GetActiveEmployeesAsync() =>
        await DbSet.Include(e => e.Department)
            .Where(e => e.IsActive)
            .OrderBy(e => e.LastName)
            .ToListAsync();

    public async Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null) =>
        await DbSet.AnyAsync(e => e.Email == email && (excludeEmployeeId == null || e.EmployeeId != excludeEmployeeId));
}
