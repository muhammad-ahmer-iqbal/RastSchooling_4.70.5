IF OBJECT_ID('sp_AddCustomerRole', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [sp_AddCustomerRole]
END
GO




CREATE PROCEDURE sp_AddCustomerRole
	@SystemName VARCHAR(MAX),
	@Name VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	IF @SystemName = '' OR @Name = ''
	BEGIN
		RETURN;
	END

    IF NOT EXISTS (SELECT * FROM CustomerRole WHERE SystemName = @SystemName)
	BEGIN
		INSERT INTO CustomerRole (Name, SystemName, FreeShipping, TaxExempt, Active,IsSystemRole,EnablePasswordLifetime, OverrideTaxDisplayType, DefaultTaxDisplayTypeId,PurchasedWithProductId) VALUES
		(@Name, @SystemName, 0, 0, 1, 0, 0, 0, 0, 0)
	END
END
GO




