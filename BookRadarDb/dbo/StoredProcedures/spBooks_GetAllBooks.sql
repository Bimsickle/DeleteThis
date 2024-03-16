create procedure [dbo].[spBooks_GetAllBooks]
		@AccountId NVARCHAR(450) = NULL,
		@PageNumber INT = 1,
		@PageSize INT = 20,
		@Search VARCHAR(255) = NULL,
		@Days INT = 21, 
		@Roles VARCHAR(255) = NULL
as
begin
	DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

	select b.Id, CreatedDate, BookTitle, Author, p.PublishingHouse, Url, ReleaseDate, 
			IsReleased, IsFeatured, IsDeleted,TotalRunSize,EditionStandard,EditionNumbered,
			EditionLettered,EditionDelux, 
			CASE WHEN EXISTS (SELECT 1 FROM STRING_SPLIT(@Roles, ',') WHERE value = 'Demigod') THEN wl.Watchers
				WHEN EXISTS (SELECT 1 FROM STRING_SPLIT(@Roles, ',') WHERE value = 'Publisher') AND uap.[PublisherId] IS NOT NULL THEN wl.Watchers
				ELSE NULL END WatchCount
			
			,NULLIF(wl_user.watched,0) IsWatched, ImageCover, ImageFeature, b.PublishingHouseId, [Description]
	
	FROM dbo.Book b
	LEFT JOIN (SELECT Count(Id)Watchers, BookId  FROM WatchList WHERE IsDeleted = 0 GROUP By BookId) wl on wl.BookId = b.Id
	LEFT JOIN (SELECT BookId, 1 watched from WatchList where UserAccountId = @AccountId AND IsDeleted<>1) wl_user on wl_user.BookId = b.Id
	LEFT JOIN (SELECT STRING_AGG(Genre, ', ') AS Genre, bg.BookId FROM BookGenre bg 
				LEFT JOIN Genre g on g.Id = bg.GenreId group by bg.BookId) g on g.BookId = b.Id

	LEFT JOIN (SELECT PublishingHouse, Id Pid FROM dbo.Publisher) p on p.Pid = b.PublishingHouseId

	LEFT JOIN (SELECT [UserAccountId] ,[PublisherId] FROM [dbo].[PublisherUser] where [IsDeleted] <> 1
				) uap ON uap.[PublisherId] = b.PublishingHouseId AND [UserAccountId] = @AccountId 

					
	
	where IsDeleted <>1

	-- Limit released books?
	AND (b.[ReleaseDate] >= DATEADD(day, -(@Days), getdate()) or b.[ReleaseDate] is null)

	-- Add Search
	AND (@Search IS NULL 
		OR BookTitle like '%'+@search+'%' 
		OR Author like '%'+@search+'%' 
		OR p.PublishingHouse like '%'+@search+'%' 
		OR g.Genre like '%'+@search+'%')

	ORDER BY NULLIF(wl_user.watched,0) desc, CreatedDate desc, ReleaseDate ASC, BookTitle
	
	OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
end