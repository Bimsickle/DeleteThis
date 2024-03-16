create procedure [dbo].[spBooks_GetOneBook]
	@BookId uniqueidentifier,
	@AccountId NVARCHAR(450) = NULL, 
	@Roles VARCHAR(255) = NULL
as
begin
	select b.Id, CreatedDate, BookTitle, Author, p.PublishingHouse, Url, ReleaseDate, 
		IsReleased, IsFeatured, IsDeleted,TotalRunSize,EditionStandard,EditionNumbered,
		EditionLettered,EditionDelux, 
		
		CASE WHEN EXISTS (SELECT 1 FROM STRING_SPLIT(@Roles, ',') WHERE value = 'Demigod') THEN wl.Watchers
				WHEN EXISTS (SELECT 1 FROM STRING_SPLIT(@Roles, ',') WHERE value = 'Publisher')  AND uap.[PublisherId] IS NOT NULL THEN wl.Watchers
				ELSE NULL END WatchCount, 
		ImageCover, ImageFeature,NULLIF(wl_user.watched,0) IsWatched, b.PublishingHouseId, [Description]
	from dbo.Book b
	LEFT JOIN (SELECT Count(Id) Watchers, BookId  FROM WatchList WHERE IsDeleted = 0 GROUP By BookId) wl on wl.BookId = b.Id
	LEFT JOIN (SELECT PublishingHouse, Id Pid FROM dbo.Publisher) p on p.Pid = b.PublishingHouseId

	LEFT JOIN (SELECT [UserAccountId] ,[PublisherId] FROM [dbo].[PublisherUser] where [IsDeleted] <> 1
				) uap ON uap.[PublisherId] = b.PublishingHouseId AND [UserAccountId] = @AccountId
	
	LEFT JOIN (SELECT BookId, 1 watched from WatchList where UserAccountId = @AccountId AND IsDeleted<>1) wl_user on wl_user.BookId = b.Id

	
	where b.IsDeleted = 0 and b.Id = @BookId;
end