# Employee Management System

A layered ASP.NET Core 8 Web API for managing employees and departments, built with
Entity Framework Core (Code First), the Repository Pattern, and SOLID principles.

## Project Overview

The system exposes REST APIs to manage `Employee` and `Department` records, including
CRUD operations, name search, department filtering, and active-employee lookups. Data
access is backed by SQL Server via EF Core, with a database view, a scalar function,
and two stored procedures included as first-class database objects. The solution is
organized into four layers plus a test project so responsibilities stay isolated and
testable:

```
EmployeeManagementSystem.sln
├── src/
│   ├── EmployeeManagementSystem.Domain          # Entities, repository interfaces, domain exceptions
│   ├── EmployeeManagementSystem.Application     # DTOs, service interfaces/implementations, business rules
│   ├── EmployeeManagementSystem.Infrastructure   # EF Core DbContext, entity configs, repositories, migrations
│   └── EmployeeManagementSystem.API             # Controllers, Swagger, DI wiring, middleware
├── tests/
│   └── EmployeeManagementSystem.Tests           # xUnit + Moq + FluentAssertions unit tests
├── database/
│   └── scripts/                                 # Standalone SQL scripts (view, function, procedures, full schema)
└── .github/workflows/ci.yml                     # CI: restore, build, test
```

Dependencies flow inward: `API` → `Application` + `Infrastructure` → `Domain`.
`Domain` has no dependencies on any other project.

## Technology Stack

- ASP.NET Core 8 Web API (C#)
- Entity Framework Core 8 (Code First, SQL Server provider)
- SQL Server (LocalDB by default)
- Swagger / Swashbuckle for API documentation
- xUnit, Moq, FluentAssertions for unit testing
- Repository Pattern + Dependency Injection

## Database Setup Steps

1. Install SQL Server LocalDB (bundled with Visual Studio, or via the
   [SQL Server Express installer](https://www.microsoft.com/sql-server/sql-server-downloads) —
   choose the "LocalDB" component).
2. Confirm the connection string in
   [src/EmployeeManagementSystem.API/appsettings.json](src/EmployeeManagementSystem.API/appsettings.json)
   points at your instance (defaults to `(localdb)\MSSQLLocalDB`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```
3. Apply migrations (see below) to create `EmployeeManagementDB`, its tables, and the
   `EmployeeDetailsView` view, `GetEmployeeFullName` scalar function, and
   `GetEmployeesByDepartment` / `GetActiveEmployees` stored procedures. Seed data for
   4 departments and 6 employees is inserted automatically.
4. Alternatively, run the standalone scripts in
   [database/scripts/](database/scripts/) directly against an existing database:
   - `01_CreateView_EmployeeDetailsView.sql`
   - `02_CreateFunction_GetEmployeeFullName.sql`
   - `03_CreateProcedure_GetEmployeesByDepartment.sql`
   - `04_CreateProcedure_GetActiveEmployees.sql`
   - `00_FullMigrationScript.sql` is the complete idempotent script generated from the
     EF Core migrations (tables, constraints, seed data, and all four objects above).

## Migration Commands

Run from the repository root. Requires the `dotnet-ef` tool
(`dotnet tool install --global dotnet-ef`).

```bash
# Apply migrations and create/update the database
dotnet ef database update \
  --project src/EmployeeManagementSystem.Infrastructure \
  --startup-project src/EmployeeManagementSystem.API

# Add a new migration after model changes
dotnet ef migrations add <MigrationName> \
  --project src/EmployeeManagementSystem.Infrastructure \
  --startup-project src/EmployeeManagementSystem.API \
  --output-dir Migrations

# Regenerate the full idempotent SQL script (used for database/scripts/00_FullMigrationScript.sql)
dotnet ef migrations script \
  --project src/EmployeeManagementSystem.Infrastructure \
  --startup-project src/EmployeeManagementSystem.API \
  --idempotent \
  --output database/scripts/00_FullMigrationScript.sql
```

## API Endpoints

### Employees — `/api/employees`

| Method | Route                          | Description                              |
|--------|---------------------------------|-------------------------------------------|
| GET    | `/api/employees`               | Get all employees                        |
| GET    | `/api/employees/{id}`          | Get an employee by id                    |
| GET    | `/api/employees/search?name=`  | Search employees by first/last name      |
| GET    | `/api/employees/department/{departmentId}` | Filter employees by department |
| GET    | `/api/employees/active`        | Get active employees                     |
| POST   | `/api/employees`               | Create an employee                       |
| PUT    | `/api/employees/{id}`          | Update an employee                       |
| DELETE | `/api/employees/{id}`          | Delete an employee                       |

### Departments — `/api/departments`

| Method | Route                       | Description                |
|--------|------------------------------|-----------------------------|
| GET    | `/api/departments`          | Get all departments        |
| GET    | `/api/departments/{id}`     | Get a department by id     |
| POST   | `/api/departments`          | Create a department        |
| PUT    | `/api/departments/{id}`     | Update a department        |
| DELETE | `/api/departments/{id}`     | Delete a department (blocked if it still has employees) |

All endpoints return `application/json`. Validation failures return `400`, missing
resources return `404`, and unhandled errors return `500` — all shaped as a small
problem-details-style JSON body by `ExceptionHandlingMiddleware`.

## Steps to Run the Application

```bash
# 1. Restore and build
dotnet restore
dotnet build

# 2. Apply EF Core migrations (creates EmployeeManagementDB)
dotnet ef database update \
  --project src/EmployeeManagementSystem.Infrastructure \
  --startup-project src/EmployeeManagementSystem.API

# 3. Run the API
dotnet run --project src/EmployeeManagementSystem.API
```

Browse to `https://localhost:<port>/swagger` (or `http://localhost:<port>/swagger`)
to explore and test the API via Swagger UI — the port is printed in the console output
on startup.

### Running the tests

```bash
dotnet test tests/EmployeeManagementSystem.Tests
```

Unit tests cover `EmployeeService` and `DepartmentService` in isolation, mocking
`IEmployeeRepository` / `IDepartmentRepository` with Moq and asserting with
FluentAssertions. They also run automatically in CI on every push/PR via
[.github/workflows/ci.yml](.github/workflows/ci.yml).
