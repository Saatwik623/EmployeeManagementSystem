using EmployeeManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<EmployeeDetailsView> EmployeeDetailsView => Set<EmployeeDetailsView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<EmployeeDetailsView>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("EmployeeDetailsView");
            builder.Property(v => v.Salary).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Department>().HasData(SeedData.Departments);
        modelBuilder.Entity<Employee>().HasData(SeedData.Employees);
    }
}
