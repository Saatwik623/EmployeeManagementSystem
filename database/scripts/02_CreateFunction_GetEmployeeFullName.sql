-- Scalar Function: GetEmployeeFullName
-- Returns the full name of the employee identified by @EmployeeId.
IF OBJECT_ID('dbo.GetEmployeeFullName', 'FN') IS NOT NULL
    DROP FUNCTION dbo.GetEmployeeFullName;
GO

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
GO
