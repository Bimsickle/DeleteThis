CREATE PROCEDURE [dbo].[spUserBookCollection_Create]
	@UserAccountId NVARCHAR(450),
	@BookId UNIQUEIDENTIFIER,
	@Edition NVARCHAR(64)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[UserBookCollection]
	(UserAccountId, BookId, BookEdition)
	VALUES
	(@UserAccountId, @BookId, @Edition);
END
