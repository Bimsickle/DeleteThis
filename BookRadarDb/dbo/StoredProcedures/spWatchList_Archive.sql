CREATE PROCEDURE [dbo].[spWatchList_Archive]
	@BookId UNIQUEIDENTIFIER,
	@UserAccountId NVARCHAR(450)
AS
BEGIN
	UPDATE [dbo].[WatchList]
	SET IsDeleted = 1
	
	WHERE BookId = @BookId AND [UserAccountId] = @UserAccountId;
END
