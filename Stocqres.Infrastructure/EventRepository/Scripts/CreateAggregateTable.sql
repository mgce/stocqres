--IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @aggregateTableName AND TABLE_SCHEMA = [Customers])
--BEGIN (

--CREATE TABLE [Customers].[@aggregateTableName](
--[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL
--[AggregateId] UNIQUEIDENTIFIER default NEWID() NOT NULL
--[AggregateType] NVARCHAR(255) NOT NULL
--[Version] INT NOT NULL
--[Data] NVARCHAR(MAX) NOT NULL
--[MetaData] NVARCHAR(MAX) NOT NULL
--[CreatedAt] DATETIME NOT NULL
--CONSTRAINT PK + @@aggregateTableName PRIMARY KEY(Id)
--)
--GO

--CREATE INDEX Idx_{aggregateTableName}Events_AggregateId ON @aggregateTableName(AggregateId)
--GO

--)
--END