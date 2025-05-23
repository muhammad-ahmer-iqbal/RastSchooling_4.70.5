IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'StudentExtension' 
		AND COLUMN_NAME = 'MonthlyFee'
		AND IS_NULLABLE = 'YES')
BEGIN
ALTER TABLE StudentExtension ALTER COLUMN MonthlyFee INT NOT NULL
END
GO



IF OBJECT_ID('TeacherExtension') IS NULL
BEGIN
CREATE TABLE TeacherExtension(
Id INT NOT NULL PRIMARY KEY IDENTITY,
CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer(Id) ON DELETE CASCADE,
Salary INT NOT NULL,
ShiftId INT NOT NULL
)
END
GO

EXEC sp_AddPermission
@Name = 'Admin area. Manage Teachers', 
@SystemName = 'ManageTeachers', 
@Category = 'Configuration'
GO



