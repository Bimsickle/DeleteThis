CREATE TABLE [dbo].[Publisher]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
	[PublishingHouse] nvarchar(255) NOT NULL,
	[IsDeleted] BIT NOT NULL Default 0
)
