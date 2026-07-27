-- Stored Procedure: GetEmployeesByDepartment
-- Returns every employee that belongs to the given department.
IF OBJECT_ID('dbo.GetEmployeesByDepartment', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetEmployeesByDepartment;
GO

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
GO
