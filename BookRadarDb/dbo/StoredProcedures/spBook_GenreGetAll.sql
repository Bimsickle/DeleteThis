CREATE PROCEDURE [dbo].[spBook_GenreGetAll]
	@BookIds NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	-- Split Id list into single GUID values
	DECLARE @IdList TABLE (Id UNIQUEIDENTIFIER);

    INSERT INTO @IdList (Id)
    SELECT CAST(value AS UNIQUEIDENTIFIER)
    FROM STRING_SPLIT(@BookIds, ',');

	-- Select the genres for the given list of Ids
	SELECT bg.Id GenreId, 
	Genre, 
	BookId 
	FROM [dbo].[BookGenre] bg 
	LEFT JOIN [dbo].[Genre] g on g.Id = bg.GenreId

	WHERE bg.BookId IN (SELECT Id FROM @IdList)
	AND bg.IsDeleted = 0 AND g.IsDeleted = 0;
END
