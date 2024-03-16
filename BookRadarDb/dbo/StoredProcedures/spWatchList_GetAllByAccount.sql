CREATE PROCEDURE [dbo].[spWatchList_GetAllByAccount]
	@AccountId NVARCHAR(450)
AS
BEGIN
	SELECT b.Id, CreatedDate, BookTitle, Author, p.PublishingHouse, Url, ReleaseDate, 
			IsReleased, IsFeatured, IsDeleted,TotalRunSize,EditionStandard,EditionNumbered,
			EditionLettered,EditionDelux,
			NULLIF(wl_user.watched,0) IsWatched, ImageCover, ImageFeature, b.PublishingHouseId, [Description]
	
	
	FROM [dbo].[Book] b

	LEFT JOIN (SELECT BookId, 1 watched from WatchList where UserAccountId = @AccountId AND IsDeleted<>1) wl_user on wl_user.BookId = b.Id
	LEFT JOIN (SELECT STRING_AGG(Genre, ', ') AS Genre, bg.BookId FROM BookGenre bg 
				LEFT JOIN Genre g on g.Id = bg.GenreId group by bg.BookId) g on g.BookId = b.Id

	LEFT JOIN (SELECT PublishingHouse, Id Pid FROM dbo.Publisher) p on p.Pid = b.PublishingHouseId

	--LEFT JOIN (SELECT Id AccountId, PublishingHouseId, UserAccess FROM [dbo].[UserAccount]) uap ON uap.PublishingHouseId = b.PublishingHouseId AND AccountId = @AccountId
	--LEFT JOIN (SELECT Id, UserAccess FROM UserAccount) a ON a.Id = @AccountId
	
	WHERE b.Id IN (
		SELECT BookId
		FROM [dbo].[WatchList]
		WHERE UserAccountId = @AccountId 
		AND IsDeleted <> 1)
	AND b.IsDeleted <> 1;
END
