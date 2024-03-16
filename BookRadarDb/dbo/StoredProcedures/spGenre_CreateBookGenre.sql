CREATE PROCEDURE [dbo].[spGenre_CreateBookGenre]
	@BookId UNIQUEIDENTIFIER,
	@GenreId UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[BookGenre]
	SET [GenreId] = @GenreId
	where [BookId] = @BookId  AND GenreId = @GenreId AND IsDeleted <>1;

	-- Insert a new record if no existing record is found
	MERGE INTO [dbo].[BookGenre] AS target
	USING (
			SELECT 1 AS Placeholder
			) AS source
	ON target.BookId = @BookId AND target.GenreId = @GenreId AND target.IsDeleted <>1
	WHEN NOT MATCHED THEN
		INSERT (BookId, GenreId)
		VALUES (@BookId, @GenreId);

END
