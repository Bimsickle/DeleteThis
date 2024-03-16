CREATE PROCEDURE [dbo].[spGenre_DeleteBookGenre]
	@BookId UNIQUEIDENTIFIER,
	@GenreId UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[BookGenre]
	SET IsDeleted = 1
	WHERE BookId = @BookId AND GenreId = @GenreId;
END