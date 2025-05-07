IF COL_LENGTH('StudentExtension', 'DateOfAdmission') IS NULL
BEGIN
ALTER TABLE StudentExtension ADD DateOfAdmission DATETIME NULL
END
GO

IF OBJECT_ID('StudentLeave') IS NULL
BEGIN
CREATE TABLE StudentLeave(
Id INT NOT NULL PRIMARY KEY IDENTITY,
CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer(Id) ON DELETE CASCADE,
StartDate DATETIME NOT NULL,
EndDate DATETIME NOT NULL)
END
GO

IF COL_LENGTH('TeacherExtension', 'DateOfJoining') IS NULL
BEGIN
ALTER TABLE TeacherExtension ADD DateOfJoining DATETIME NULL
END
GO

IF OBJECT_ID('Department') IS NULL
BEGIN
CREATE TABLE Department(
Id INT NOT NULL PRIMARY KEY IDENTITY,
Name VARCHAR(500) NOT NULL,
Deleted BIT NOT NULL)
END
GO

IF OBJECT_ID('Designation') IS NULL
BEGIN
CREATE TABLE Designation(
Id INT NOT NULL PRIMARY KEY IDENTITY,
Name VARCHAR(500) NOT NULL,
Deleted BIT NOT NULL)
END
GO

IF COL_LENGTH('TeacherExtension', 'DesignationId') IS NULL
BEGIN
ALTER TABLE TeacherExtension ADD DesignationId INT NULL FOREIGN KEY REFERENCES Designation(Id) ON DELETE CASCADE
END
GO

IF COL_LENGTH('TeacherExtension', 'DepartmentId') IS NULL
BEGIN
ALTER TABLE TeacherExtension ADD DepartmentId INT NULL FOREIGN KEY REFERENCES Department(Id) ON DELETE CASCADE
END
GO

IF NOT EXISTS (SELECT * FROM PermissionRecord WHERE SystemName = 'ManageDepartments')
BEGIN
INSERT INTO PermissionRecord (Name,SystemName,Category) VALUES
('Admin area. Manage Departments', 'ManageDepartments', 'Configuration')
END
GO

IF NOT EXISTS (SELECT * FROM PermissionRecord WHERE SystemName = 'ManageDesignations')
BEGIN
INSERT INTO PermissionRecord (Name,SystemName,Category) VALUES
('Admin area. Manage Designations', 'ManageDesignations', 'Configuration')
END
GO



