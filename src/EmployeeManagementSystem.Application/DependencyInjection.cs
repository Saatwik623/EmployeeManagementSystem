using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        return services;
    }
}
