CREATE PROCEDURE [dbo].[spWatchList_Create]
	@UserAccount NVARCHAR(450),
	@BookId UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE [dbo].[WatchList]
		SET IsDeleted = 0
		WHERE BookId = @BookId AND [UserAccountId] = @UserAccount;

		-- Insert a new record if no existing record is found
		MERGE INTO [dbo].[WatchList] AS target
		USING (
			SELECT 1 AS Placeholder
		) AS source
		ON target.BookId = @BookId AND target.[UserAccountId] = @UserAccount
		WHEN NOT MATCHED THEN
			INSERT (UserAccountId, BookId)
			VALUES (@UserAccount, @BookId);
END
