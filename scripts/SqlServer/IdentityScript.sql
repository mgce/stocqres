
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Identity')
BEGIN
	EXEC('CREATE SCHEMA [Identity]');
END

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Customers')
BEGIN
	EXEC('CREATE SCHEMA [Customers]');
END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Identity' 
                 AND  TABLE_NAME = 'User'))
BEGIN
		CREATE TABLE [Identity].[User](
		[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
		[Username] NVARCHAR(255),
		[Email] NVARCHAR(255),
		[Password] NVARCHAR(255),
		[CreatedAt] DATETIME,
		[ModifiedAt] DATETIME,
		[State] INT
	)
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Identity' 
                 AND  TABLE_NAME = 'RefreshToken'))
BEGIN
	CREATE TABLE [Identity].[RefreshToken](
		[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
		[UserId] UNIQUEIDENTIFIER default NEWID(),
		[Token] NVARCHAR(255),
		[Email] NVARCHAR(255),
		[Revoked] BIT DEFAULT 0,
		[RevokedAt] DATETIME,
		[CreatedAt] DATETIME,
		[ModifiedAt] DATETIME,
		[State] INT
	)
END

