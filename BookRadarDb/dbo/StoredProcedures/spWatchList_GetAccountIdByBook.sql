CREATE PROCEDURE [dbo].[spWatchList_GetAccountIdByBook]
	@BookId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT UserAccountId [Id]
		FROM [dbo].[WatchList]
		WHERE BookId = @BookId 
		AND IsDeleted = 0
END