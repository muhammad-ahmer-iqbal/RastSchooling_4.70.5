IF OBJECT_ID('Form') IS NULL
BEGIN
CREATE TABLE Form(
Id INT NOT NULL PRIMARY KEY IDENTITY,
Name VARCHAR(500) NOT NULL,
Active BIT NOT NULL)
END
GO


EXEC sp_AddPermission
@Name = 'Admin area. Manage Forms', 
@SystemName = 'ManageForms', 
@Category = 'Configuration'
GO

IF OBJECT_ID('FormField') IS NULL
BEGIN
CREATE TABLE FormField(
Id INT NOT NULL PRIMARY KEY IDENTITY,
Name VARCHAR(500) NOT NULL,
DisplayOrder INT NOT NULL,
ControlTypeId INT NOT NULL,
Required BIT NOT NULL,
FormId INT NOT NULL FOREIGN KEY REFERENCES Form(Id) ON DELETE CASCADE)
END
GO

IF OBJECT_ID('FormFieldOption') IS NULL
BEGIN
CREATE TABLE FormFieldOption(
Id INT NOT NULL PRIMARY KEY IDENTITY,
Name VARCHAR(500) NOT NULL,
DisplayOrder INT NOT NULL,
FormFieldId INT NOT NULL FOREIGN KEY REFERENCES FormField(Id) ON DELETE CASCADE)
END
GO

