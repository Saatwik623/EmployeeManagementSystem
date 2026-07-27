# AI Usage Report

This project was scaffolded end-to-end by **Claude Code** (Anthropic's AI coding
assistant) in a single collaborative session, filling the role GitHub Copilot / ChatGPT
would play under the assignment brief. This report documents two required use cases in
detail — **generating unit tests** and **generating XML/API documentation** — plus a
note on repository/service generation, all drawn from what actually happened in the
session rather than a hypothetical example.

## 1. Generate Unit Tests

**AI prompt used** (the operative instruction from the assignment brief, supplied at
the start of the session):

> "Write unit tests for EmployeeService, DepartmentService. Use xUnit, Moq,
> FluentAssertions. Target minimum 70% code coverage."

Claude Code turned this into a concrete test plan covering every public method and
branch of both services (happy path plus each validation/not-found rule) before
writing the test files.

**Generated output:**
- [tests/EmployeeManagementSystem.Tests/Services/EmployeeServiceTests.cs](tests/EmployeeManagementSystem.Tests/Services/EmployeeServiceTests.cs)
  — 15 tests covering add/update/delete/get-by-id/get-all/search/filter-by-department/
  get-active, including the validation paths for a non-existent department and a
  duplicate email on both create and update.
- [tests/EmployeeManagementSystem.Tests/Services/DepartmentServiceTests.cs](tests/EmployeeManagementSystem.Tests/Services/DepartmentServiceTests.cs)
  — 11 tests covering add/update/delete/get-by-id/get-all, including the rule that a
  department still holding employees cannot be deleted.

**Manual verification / modifications made:**
- The test and solution build were verified with `dotnet build`, which succeeded with
  zero warnings/errors — confirming the generated mocks (`Mock<IEmployeeRepository>`,
  `Mock<IDepartmentRepository>`, `Mock<ILogger<T>>`) match the actual repository/service
  signatures.
- Local `dotnet test` execution failed on the development machine with "An Application
  Control policy has blocked this file" — this is Windows Smart App Control (WDAC)
  blocking the VSTest test host from loading the freshly-built test assembly, unrelated
  to test correctness. Rather than disabling that machine-wide security policy (which
  Microsoft documents as effectively irreversible without a Windows reset once
  enforced), a GitHub Actions workflow
  ([.github/workflows/ci.yml](.github/workflows/ci.yml)) was added so
  `dotnet build` + `dotnet test` run on Linux CI runners on every push/PR instead.

## 2. Generate XML/API Documentation

**AI prompt used:**

> "Enable Swagger for API documentation and testing" plus "Generate XML/API
> documentation" from the assignment's AI-assisted development requirements.

**Generated output:**
- `<summary>` XML doc comments on every action in
  [EmployeesController.cs](src/EmployeeManagementSystem.API/Controllers/EmployeesController.cs)
  and [DepartmentsController.cs](src/EmployeeManagementSystem.API/Controllers/DepartmentsController.cs),
  plus a class-level summary on each controller.
- `GenerateDocumentationFile` and `NoWarn` (for CS1591, "missing XML comment") added to
  [EmployeeManagementSystem.API.csproj](src/EmployeeManagementSystem.API/EmployeeManagementSystem.API.csproj).
- Swagger wired in [Program.cs](src/EmployeeManagementSystem.API/Program.cs) to load
  that generated XML file via `options.IncludeXmlComments(xmlPath)`, guarded by a
  `File.Exists` check so a first-time checkout that hasn't built yet doesn't crash
  Swagger generation.

**Manual verification made:**
- Confirmed via `dotnet build` that `EmployeeManagementSystem.API.xml` is produced
  alongside the DLL in `bin/Debug/net8.0/`, and that the conditional `IncludeXmlComments`
  call compiles cleanly.

## 3. Repository & Migration Generation (supplementary)

The `Repository<T>` generic base class, `EmployeeRepository`, `DepartmentRepository`,
and the EF Core migration were also AI-authored from the domain interfaces and entity
configurations. One real, concrete correction happened during this step: running
`dotnet ef migrations add InitialCreate` produced a warning that the keyless
`EmployeeDetailsView` entity's `Salary` property had no explicit store type ("This will
cause values to be silently truncated..."). The fix — adding
`builder.Property(v => v.Salary).HasColumnType("decimal(18,2)")` to the view's
configuration in `AppDbContext.OnModelCreating` — was applied, and the migration was
removed and regenerated cleanly with zero warnings.

## Summary

Every file in this repository was AI-generated in this session and then verified by
building the solution (`dotnet build`, zero warnings/errors) and, for the API, running
it locally (`dotnet run`, confirmed it starts and listens). The two concrete issues
encountered — the view's decimal precision warning and the local test-execution block —
are both documented above with their actual resolutions, rather than a hypothetical
example.
