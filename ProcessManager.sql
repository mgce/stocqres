CREATE SCHEMA [Transactions]
GO

CREATE TABLE [Transactions].[OrderProcessManager](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	[AggregateId] UNIQUEIDENTIFIER NULL, 
	[WalletId] UNIQUEIDENTIFIER NULL,
	[CompanyId] UNIQUEIDENTIFIER NULL,
	[StockName] NVARCHAR(255) NULL,
	[StockCode] NVARCHAR(255) NULL,
	[StockUnit] INT NULL,
	[StockQuantity] INT NULL,
	[ChargedWalletAmount] DECIMAL(10,2) NULL,
	[CreatedAt] DATETIME NULL,
	[ModifiedAt] DATETIME NULL,
	[State] INT  NULL
)