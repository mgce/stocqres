
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Transactions')
BEGIN
	EXEC('CREATE SCHEMA [Transactions]');
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Transaction' 
                 AND  TABLE_NAME = 'BuyOrderProcessManage'))
BEGIN
	CREATE TABLE [Transactions].[BuyOrderProcessManager](
		[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
		[AggregateId] UNIQUEIDENTIFIER NULL, 
		[WalletId] UNIQUEIDENTIFIER NULL,
		[CompanyId] UNIQUEIDENTIFIER NULL,
		[StockName] NVARCHAR(255) NULL,
		[StockCode] NVARCHAR(255) NULL,
		[StockUnit] INT NULL,
		[StockQuantity] INT NULL,
		[ChargedWalletAmount] DECIMAL(10,2) NULL,
		[CancelReason] NVARCHAR(Max) NULL,
		[State] TINYINT NULL,
		[CreatedAt] DATETIME NULL,
		[ModifiedAt] DATETIME NULL,
	)
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Transaction' 
                 AND  TABLE_NAME = 'SellOrderProcessManage'))
BEGIN
	CREATE TABLE [Transactions].[SellOrderProcessManager](
		[Id] UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
		[AggregateId] UNIQUEIDENTIFIER NULL, 
		[WalletId] UNIQUEIDENTIFIER NULL,
		[CompanyId] UNIQUEIDENTIFIER NULL,
		[StockCode] NVARCHAR(255) NULL,
		[StockQuantity] INT NULL,
		[CancelReason] NVARCHAR(Max) NULL,
		[State] TINYINT  NULL,
		[CreatedAt] DATETIME NULL,
		[ModifiedAt] DATETIME NULL,
	)
END