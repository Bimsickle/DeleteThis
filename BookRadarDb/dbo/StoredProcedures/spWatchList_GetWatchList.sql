CREATE PROCEDURE [dbo].[spWatchList_GetWatchList]
	@accountId UNIQUEIDENTIFIER 
AS
BEGIN
	SET NOCOUNT ON

	SELECT
	 wl.[Id]
    ,wl.[CreatedDate]      
    ,[BookId]
      
  FROM [dbo].[WatchList] wl
  LEFT JOIN [dbo].[Book] b on b.Id = wl.BookId

  WHERE wl.[IsDeleted] = 0
  AND [UserAccountId] = @accountId
  AND b.[IsDeleted] = 0

  ORDER BY wl.CreatedDate DESC;
END
