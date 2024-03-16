CREATE PROCEDURE [dbo].[spGenre_GetOrCreateGenreByGenre]
	@Genre NVARCHAR(255) 

AS

BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Genre]
	SET Genre = @Genre
	WHERE Genre = @Genre AND IsDeleted<>1;

	-- Insert a new record if no existing record is found
	MERGE INTO [dbo].[Genre] AS target
	USING (
			SELECT 1 AS Placeholder
			) AS source
	ON target.Genre = @Genre AND target.IsDeleted <>1
	WHEN NOT MATCHED THEN
			INSERT (Genre)
			VALUES (@genre);

	SET NOCOUNT ON;

	SELECT [Id] ,[Genre] ,[IsDeleted]
	FROM [dbo].[Genre]
	WHERE IsDeleted <>1 and Genre = @Genre;

END