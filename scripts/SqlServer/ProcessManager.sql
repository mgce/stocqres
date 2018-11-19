
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Transactions')
BEGIN
	EXEC('CREATE SCHEMA [Transactions]');
END

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Transaction' 
                 AND  TABLE_NAME = 'OrderProcessManage'))
BEGIN
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
		[CancelReason] NVARCHAR(Max) NULL,
		[CreatedAt] DATETIME NULL,
		[ModifiedAt] DATETIME NULL,
		[State] INT  NULL
	)
END