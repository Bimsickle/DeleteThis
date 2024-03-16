﻿CREATE TABLE [dbo].[WatchList]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
	[UserAccountId] NVARCHAR(450) NOT NULL,
	[BookId] UNIQUEIDENTIFIER NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
)
