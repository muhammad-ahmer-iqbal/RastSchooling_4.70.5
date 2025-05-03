IF OBJECT_ID('StudentExtension') IS NULL
BEGIN
CREATE TABLE StudentExtension(
Id INT NOT NULL PRIMARY KEY IDENTITY,
CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer(Id) ON DELETE CASCADE,
MonthlyFee INT NULL
)
END
GO

IF NOT EXISTS (SELECT * FROM PermissionRecord WHERE SystemName = 'ManageStudents')
BEGIN
INSERT INTO PermissionRecord (Name,SystemName,Category) VALUES
('Admin area. Manage Students', 'ManageStudents', 'Configuration')
END
GO

EXEC sp_AddCustomerRole
@SystemName = 'RastAdmin',
@Name = 'Rast Admin'
GO

EXEC sp_AddCustomerRole
@SystemName = 'Students',
@Name = 'Students'
GO

EXEC sp_AddCustomerRole
@SystemName = 'Teachers',
@Name = 'Teachers'
GO


