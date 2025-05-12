IF OBJECT_ID('sp_AddPermission', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [sp_AddPermission]
END
GO



CREATE PROCEDURE sp_AddPermission
	@Name VARCHAR(MAX), 
	@SystemName VARCHAR(MAX), 
	@Category VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@SystemName, '') = '' OR ISNULL(@Name, '') = ''
	BEGIN
		RETURN;
	END

    
	IF NOT EXISTS (SELECT * FROM PermissionRecord WHERE SystemName = @SystemName)
	BEGIN
		INSERT INTO PermissionRecord (Name, SystemName, Category) VALUES
		(@Name, @SystemName, @Category)
	END

	DECLARE @Permission_Id INT  = (SELECT Id FROM PermissionRecord WHERE SystemName = @SystemName),
	@Admin_Role_Id INT  = (SELECT Id FROM CustomerRole WHERE SystemName = 'Administrators');

	INSERT INTO PermissionRecord_Role_Mapping (CustomerRole_Id, PermissionRecord_Id)
	SELECT CCRM.Customer_Id, @Permission_Id FROM Customer_CustomerRole_Mapping CCRM
	WHERE CCRM.CustomerRole_Id = @Admin_Role_Id
	AND NOT EXISTS (SELECT * FROM PermissionRecord_Role_Mapping PRRM WHERE PRRM.PermissionRecord_Id = @Permission_Id
		AND PRRM.CustomerRole_Id = @Admin_Role_Id)

END
GO







