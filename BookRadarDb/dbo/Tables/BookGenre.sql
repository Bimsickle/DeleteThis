CREATE TABLE [dbo].[BookGenre]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
	[BookId] [uniqueidentifier] NOT NULL,
	[GenreId] [uniqueidentifier] NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0 

	-- Define foreign key constraints
	FOREIGN KEY (BookId) REFERENCES Book(Id), 
	FOREIGN KEY (GenreId) REFERENCES Genre(Id)
)
