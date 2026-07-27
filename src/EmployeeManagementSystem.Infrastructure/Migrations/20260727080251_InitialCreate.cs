using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.CheckConstraint("CK_Employee_Salary", "[Salary] >= 0");
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "CreatedDate", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Builds and maintains software products.", "Engineering" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manages hiring, payroll and employee relations.", "Human Resources" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Drives revenue through customer acquisition.", "Sales" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manages budgeting, accounting and financial planning.", "Finance" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "DepartmentId", "Email", "FirstName", "HireDate", "IsActive", "LastName", "PhoneNumber", "Salary" },
                values: new object[,]
                {
                    { 1, 1, "ava.thompson@example.com", "Ava", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Thompson", "555-0101", 95000m },
                    { 2, 1, "liam.chen@example.com", "Liam", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Chen", "555-0102", 88000m },
                    { 3, 2, "sophia.martinez@example.com", "Sophia", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Martinez", "555-0103", 72000m }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "DepartmentId", "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { 4, 3, "noah.patel@example.com", "Noah", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Patel", "555-0104", 68000m });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "DepartmentId", "Email", "FirstName", "HireDate", "IsActive", "LastName", "PhoneNumber", "Salary" },
                values: new object[,]
                {
                    { 5, 4, "isabella.nguyen@example.com", "Isabella", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Nguyen", "555-0105", 79000m },
                    { 6, 1, "mason.brown@example.com", "Mason", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Brown", "555-0106", 91000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_Name",
                table: "Department",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                table: "Employee",
                column: "Email",
                unique: true);

            migrationBuilder.Sql(@"
CREATE VIEW dbo.EmployeeDetailsView AS
SELECT
    e.EmployeeId,
    e.FirstName + ' ' + e.LastName AS FullName,
    e.Email,
    e.Salary,
    e.HireDate,
    e.IsActive,
    d.Name AS DepartmentName
FROM dbo.Employee e
INNER JOIN dbo.Department d ON e.DepartmentId = d.DepartmentId;
");

            migrationBuilder.Sql(@"
CREATE FUNCTION dbo.GetEmployeeFullName (@EmployeeId INT)
RETURNS NVARCHAR(101)
AS
BEGIN
    DECLARE @FullName NVARCHAR(101);

    SELECT @FullName = FirstName + ' ' + LastName
    FROM dbo.Employee
    WHERE EmployeeId = @EmployeeId;

    RETURN @FullName;
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.GetEmployeesByDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.PhoneNumber,
        e.Salary,
        e.HireDate,
        e.IsActive,
        e.DepartmentId,
        d.Name AS DepartmentName
    FROM dbo.Employee e
    INNER JOIN dbo.Department d ON e.DepartmentId = d.DepartmentId
    WHERE e.DepartmentId = @DepartmentId
    ORDER BY e.LastName;
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.GetActiveEmployees
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.PhoneNumber,
        e.Salary,
        e.HireDate,
        e.IsActive,
        e.DepartmentId,
        d.Name AS DepartmentName
    FROM dbo.Employee e
    INNER JOIN dbo.Department d ON e.DepartmentId = d.DepartmentId
    WHERE e.IsActive = 1
    ORDER BY e.LastName;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetActiveEmployees;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetEmployeesByDepartment;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.GetEmployeeFullName;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.EmployeeDetailsView;");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
