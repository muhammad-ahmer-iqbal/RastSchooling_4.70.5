IF OBJECT_ID('sp_AddLocalization', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [sp_AddLocalization]
END
GO




CREATE PROCEDURE sp_AddLocalization
	-- Add the parameters for the stored procedure here
	@ResourceName VARCHAR(MAX),
	@ResourceValue VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ResourceName = '' OR @ResourceValue = ''
	BEGIN
		RETURN;
	END

    IF NOT EXISTS (SELECT * FROM LocaleStringResource WHERE ResourceName = @ResourceName)
	BEGIN
		INSERT INTO LocaleStringResource (ResourceName, ResourceValue, LanguageId)
		SELECT @ResourceName, @ResourceValue, Id FROM [Language]
	END
	ELSE
	BEGIN
		UPDATE LocaleStringResource SET ResourceValue = @ResourceValue WHERE ResourceName = @ResourceName
	END
END
GO

