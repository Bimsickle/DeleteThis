CREATE PROCEDURE [dbo].[spGenre_GetAllGenres]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id] ,[Genre] ,[IsDeleted]
	FROM [dbo].[Genre]
	WHERE IsDeleted <>1
	ORDER BY Genre ASC;

END