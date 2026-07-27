IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    CREATE TABLE [Department] (
        [DepartmentId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(250) NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Department] PRIMARY KEY ([DepartmentId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    CREATE TABLE [Employee] (
        [EmployeeId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [PhoneNumber] nvarchar(20) NULL,
        [Salary] decimal(18,2) NOT NULL,
        [HireDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [DepartmentId] int NOT NULL,
        CONSTRAINT [PK_Employee] PRIMARY KEY ([EmployeeId]),
        CONSTRAINT [CK_Employee_Salary] CHECK ([Salary] >= 0),
        CONSTRAINT [FK_Employee_Department_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Department] ([DepartmentId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DepartmentId', N'CreatedDate', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Department]'))
        SET IDENTITY_INSERT [Department] ON;
    EXEC(N'INSERT INTO [Department] ([DepartmentId], [CreatedDate], [Description], [Name])
    VALUES (1, ''2026-01-01T00:00:00.0000000Z'', N''Builds and maintains software products.'', N''Engineering''),
    (2, ''2026-01-01T00:00:00.0000000Z'', N''Manages hiring, payroll and employee relations.'', N''Human Resources''),
    (3, ''2026-01-01T00:00:00.0000000Z'', N''Drives revenue through customer acquisition.'', N''Sales''),
    (4, ''2026-01-01T00:00:00.0000000Z'', N''Manages budgeting, accounting and financial planning.'', N''Finance'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DepartmentId', N'CreatedDate', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Department]'))
        SET IDENTITY_INSERT [Department] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'IsActive', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] ON;
    EXEC(N'INSERT INTO [Employee] ([EmployeeId], [DepartmentId], [Email], [FirstName], [HireDate], [IsActive], [LastName], [PhoneNumber], [Salary])
    VALUES (1, 1, N''ava.thompson@example.com'', N''Ava'', ''2026-01-01T00:00:00.0000000Z'', CAST(1 AS bit), N''Thompson'', N''555-0101'', 95000.0),
    (2, 1, N''liam.chen@example.com'', N''Liam'', ''2026-01-01T00:00:00.0000000Z'', CAST(1 AS bit), N''Chen'', N''555-0102'', 88000.0),
    (3, 2, N''sophia.martinez@example.com'', N''Sophia'', ''2026-01-01T00:00:00.0000000Z'', CAST(1 AS bit), N''Martinez'', N''555-0103'', 72000.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'IsActive', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] ON;
    EXEC(N'INSERT INTO [Employee] ([EmployeeId], [DepartmentId], [Email], [FirstName], [HireDate], [LastName], [PhoneNumber], [Salary])
    VALUES (4, 3, N''noah.patel@example.com'', N''Noah'', ''2026-01-01T00:00:00.0000000Z'', N''Patel'', N''555-0104'', 68000.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'IsActive', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] ON;
    EXEC(N'INSERT INTO [Employee] ([EmployeeId], [DepartmentId], [Email], [FirstName], [HireDate], [IsActive], [LastName], [PhoneNumber], [Salary])
    VALUES (5, 4, N''isabella.nguyen@example.com'', N''Isabella'', ''2026-01-01T00:00:00.0000000Z'', CAST(1 AS bit), N''Nguyen'', N''555-0105'', 79000.0),
    (6, 1, N''mason.brown@example.com'', N''Mason'', ''2026-01-01T00:00:00.0000000Z'', CAST(1 AS bit), N''Brown'', N''555-0106'', 91000.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'DepartmentId', N'Email', N'FirstName', N'HireDate', N'IsActive', N'LastName', N'PhoneNumber', N'Salary') AND [object_id] = OBJECT_ID(N'[Employee]'))
        SET IDENTITY_INSERT [Employee] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Department_Name] ON [Department] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Employee_DepartmentId] ON [Employee] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employee_Email] ON [Employee] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727080251_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260727080251_InitialCreate', N'8.0.10');
END;
GO

COMMIT;
GO

