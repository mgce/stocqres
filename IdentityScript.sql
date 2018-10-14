CREATE SCHEMA [Identity]
GO

CREATE SCHEMA [Customers]
GO

CREATE TABLE [Identity].[User](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	[Username] NVARCHAR(255),
	[Email] NVARCHAR(255),
	[Password] NVARCHAR(255),
	[CreatedAt] DATETIME,
	[ModifiedAt] DATETIME,
	[State] INT
)

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