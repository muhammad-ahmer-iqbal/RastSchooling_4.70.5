IF EXISTS (SELECT * FROM CustomerRole WHERE SystemName IN ('ForumModerators','Vendors'))
BEGIN
DELETE FROM CustomerRole WHERE SystemName IN ('ForumModerators','Vendors')
END
GO

