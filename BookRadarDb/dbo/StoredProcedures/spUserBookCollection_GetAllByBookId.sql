CREATE PROCEDURE [dbo].[spUserBookCollection_GetAllByBookId]
	@BookId UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	ubc.BookEdition, ubc.UserAccountId UserName
	FROM [dbo].[UserBookCollection] ubc
	LEFT JOIN Book b on b.Id = ubc.BookId
	
	WHERE ubc.BookId = @BookId
	AND ubc.IsDeleted <>1;
END
