CREATE TABLE [dbo].[NewsArticle]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[Title] nvarchar(255) NOT NULL,
	[Content] nvarchar(MAX) NOT NULL,
	[AuthorId] NVARCHAR(450) NOT NULL,
	[PublishDate] DATE DEFAULT GETUTCDATE(),
	[Tags] NVARCHAR(255) DEFAULT '#News',
	[PreviewImage] NVARCHAR(MAX) NULL,
	[BannerImage] NVARCHAR(MAX) NULL,
	[IsPublished] BIT DEFAULT 0,
	[IsDeleted] BIT DEFAULT 0
	)
