-- View: EmployeeDetailsView
-- Combines employee and department data into a single denormalized read view.
IF OBJECT_ID('dbo.EmployeeDetailsView', 'V') IS NOT NULL
    DROP VIEW dbo.EmployeeDetailsView;
GO

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
GO
