using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;
using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<Department>> GetAllAsync() =>
        await DbSet.Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToListAsync();

    public async Task<bool> NameExistsAsync(string name, int? excludeDepartmentId = null) =>
        await DbSet.AnyAsync(d => d.Name == name && (excludeDepartmentId == null || d.DepartmentId != excludeDepartmentId));

    public async Task<bool> HasEmployeesAsync(int departmentId) =>
        await Context.Employees.AnyAsync(e => e.DepartmentId == departmentId);
}
