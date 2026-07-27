-- Stored Procedure: GetActiveEmployees
-- Returns every employee currently marked as active.
IF OBJECT_ID('dbo.GetActiveEmployees', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetActiveEmployees;
GO

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
GO
