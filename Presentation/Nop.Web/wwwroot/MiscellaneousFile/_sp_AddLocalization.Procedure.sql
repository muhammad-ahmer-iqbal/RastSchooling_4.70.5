IF OBJECT_ID('sp_AddLocalization', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [sp_AddLocalization]
END
GO




CREATE PROCEDURE sp_AddLocalization
	@ResourceName VARCHAR(MAX),
	@ResourceValue VARCHAR(MAX)
AS
BEGIN
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




