IF COL_LENGTH('StudentExtension', 'HouseId') IS NULL
BEGIN
ALTER TABLE StudentExtension ADD HouseId INT NULL
END
GO

IF OBJECT_ID('StudentSessionMapping') IS NULL
BEGIN
CREATE TABLE StudentSessionMapping(
Id INT NOT NULL IDENTITY PRIMARY KEY,
CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer(Id) ON DELETE CASCADE,
DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Department(Id) ON DELETE CASCADE
)
END
GO

IF COL_LENGTH('Form', 'DepartmentId') IS NULL
BEGIN
ALTER TABLE Form ADD DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Department(Id) ON DELETE CASCADE DEFAULT(1)
END
GO

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TeacherExtension'
      AND COLUMN_NAME = 'DepartmentId'
      AND IS_NULLABLE = 'YES'
)
BEGIN
ALTER TABLE TeacherExtension ALTER COLUMN DepartmentId INT NOT NULL
END
GO


IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TeacherExtension'
      AND COLUMN_NAME = 'DesignationId'
      AND IS_NULLABLE = 'YES'
)
BEGIN
ALTER TABLE TeacherExtension ALTER COLUMN DesignationId INT NOT NULL
END
GO