CREATE PROCEDURE [dbo].[spAccessLog_GetByUserId]
	@StartDate datetime,
	@EndDate datetime,
	@UserId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	al.[CreatedDate] ,
	al.[UserId] ,
	al.[UserName] ,
	al.[EventAction],
	al.[IdType] ,
	al.[TypeId] ,
	CASE WHEN al.[IdType] = 'Book' THEN b.BookTitle
			WHEN al.[IdType] = 'News' THEN n.Title
			-- add whatever other logic here
			END AS [Description]

	FROM
	[dbo].[AccessLog] al
	LEFT JOIN dbo.Book b on b.Id = al.TypeId AND IdType = 'Book'
	LEFT JOIN dbo.NewsArticle n on n.Id = al.TypeId AND IdType = 'News'

	WHERE al.[CreatedDate] BETWEEN @StartDate AND @EndDate AND al.[UserId]  = @UserId
END