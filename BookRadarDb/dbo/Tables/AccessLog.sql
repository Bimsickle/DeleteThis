﻿CREATE TABLE [dbo].[AccessLog]
(
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[UserId] NVARCHAR(450) NOT NULL,
	[UserName] NVARCHAR(450) NOT NULL,
	[EventAction] NVARCHAR(255) NOT NULL ,
	[IdType] NVARCHAR(100) NULL,
	[TypeId] UNIQUEIDENTIFIER NULL
)
