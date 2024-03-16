CREATE TABLE [dbo].[Book]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[BookTitle] nvarchar(255) NOT NULL,
	[Author] nvarchar(255) NOT NULL,
	[Url] nvarchar(255),
	[ReleaseDate] datetime2(7) NULL,
	[IsReleased] BIT NOT NULL DEFAULT 0,
	[IsFeatured] BIT NOT NULL DEFAULT 0,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[IsConfirmed] BIT DEFAULT 0,
	[TotalRunSize] INT NOT NULL DEFAULT 0,
	[EditionStandard] INT NULL,
	[EditionNumbered] INT NULL,
	[EditionLettered] INT NULL,
	[EditionDelux] INT NULL,
	[ImageCover] NVARCHAR(MAX) DEFAULT NULL,
	[ImageFeature] NVARCHAR(MAX) DEFAULT NULL,
	[PublishingHouseId] UNIQUEIDENTIFIER NULL,
	[Description] NVARCHAR(MAX) NULL

	-- Define foreign key constraints
	FOREIGN KEY ([PublishingHouseId]) REFERENCES Publisher(Id)
)
