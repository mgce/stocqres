IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = 'InvestorEvents' AND xtype='U')
CREATE TABLE [Customers].[InvestorEvents](
[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL,
[AggregateId] UNIQUEIDENTIFIER NOT NULL,
[AggregateType] NVARCHAR(255) NOT NULL,
[Version] INT NOT NULL,
[Data] NVARCHAR(MAX) NOT NULL,
[MetaData] NVARCHAR(MAX) NOT NULL,
[CreatedAt] DATETIME NOT NULL,
CONSTRAINT PKInvestorEvents PRIMARY KEY(ID)
)
GO
IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_InvestorEvents_AggregateId' AND object_id = OBJECT_ID('[Customers].InvestorEvents'))
begin
CREATE INDEX Idx_InvestorEvents_AggregateId ON [Customers].InvestorEvents(AggregateId)
end
GO

IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = 'CompanyEvents' AND xtype='U')
CREATE TABLE [Customers].[CompanyEvents](
[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL,
[AggregateId] UNIQUEIDENTIFIER NOT NULL,
[AggregateType] NVARCHAR(255) NOT NULL,
[Version] INT NOT NULL,
[Data] NVARCHAR(MAX) NOT NULL,
[MetaData] NVARCHAR(MAX) NOT NULL,
[CreatedAt] DATETIME NOT NULL,
CONSTRAINT PKCompanyEvents PRIMARY KEY(ID)
)
GO
IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_CompanyEvents_AggregateId' AND object_id = OBJECT_ID('[Customers].CompanyEvents'))
begin
CREATE INDEX Idx_CompanyEvents_AggregateId ON [Customers].CompanyEvents(AggregateId)
end
GO

IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = 'WalletEvents' AND xtype='U')
CREATE TABLE [Customers].[WalletEvents](
[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL,
[AggregateId] UNIQUEIDENTIFIER NOT NULL,
[AggregateType] NVARCHAR(255) NOT NULL,
[Version] INT NOT NULL,
[Data] NVARCHAR(MAX) NOT NULL,
[MetaData] NVARCHAR(MAX) NOT NULL,
[CreatedAt] DATETIME NOT NULL,
CONSTRAINT PKWalletEvents PRIMARY KEY(ID)
)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_WalletEvents_AggregateId' AND object_id = OBJECT_ID('[Customers].WalletEvents'))
begin
CREATE INDEX Idx_WalletEvents_AggregateId ON [Customers].WalletEvents(AggregateId)
end
GO


IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = 'OrderEvents' AND xtype='U')
CREATE TABLE [Customers].[OrderEvents](
[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL,
[AggregateId] UNIQUEIDENTIFIER NOT NULL,
[AggregateType] NVARCHAR(255) NOT NULL,
[Version] INT NOT NULL,
[Data] NVARCHAR(MAX) NOT NULL,
[MetaData] NVARCHAR(MAX) NOT NULL,
[CreatedAt] DATETIME NOT NULL,
CONSTRAINT PKOrderEvents PRIMARY KEY(ID)
)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_OrderEvents_AggregateId' AND object_id = OBJECT_ID('[Customers].OrderEvents'))
begin
CREATE INDEX Idx_OrderEvents_AggregateId ON [Customers].OrderEvents(AggregateId)
end
GO

/*
Template:

IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = 'OrderEvents' AND xtype='U')
CREATE TABLE [Customers].[OrderEvents](
[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL,
[AggregateId] UNIQUEIDENTIFIER NOT NULL,
[AggregateType] NVARCHAR(255) NOT NULL,
[Version] INT NOT NULL,
[Data] NVARCHAR(MAX) NOT NULL,
[MetaData] NVARCHAR(MAX) NOT NULL,
[CreatedAt] DATETIME NOT NULL,
CONSTRAINT PKOrderEvents PRIMARY KEY(ID)
)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_OrderEvents_AggregateId' AND object_id = OBJECT_ID('[Customers].OrderEvents'))
begin
CREATE INDEX Idx_OrderEvents_AggregateId ON [Customers].OrderEvents(AggregateId)
end
GO


*/