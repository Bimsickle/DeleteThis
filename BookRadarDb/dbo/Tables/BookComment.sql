CREATE TABLE [dbo].[BookComment]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[BookId] UNIQUEIDENTIFIER NOT NULL,
	[AuthorId] NVARCHAR(450) NOT NULL,
	[Comment] NVARCHAR(1024) ,
	[IsApproved] BIT DEFAULT 0,
	[IsDeleted] BIT DEFAULT 0

	-- Define foreign key constraints
	FOREIGN KEY ([BookId]) REFERENCES Book(Id)
)
