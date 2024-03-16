CREATE PROCEDURE [dbo].[spBooks_GetFeatured]
	
AS

BEGIN
	SET NOCOUNT ON;

	select Id, CreatedDate, BookTitle, Author, p.PublishingHouse, Url, ReleaseDate, 
			IsReleased, IsFeatured, IsDeleted,TotalRunSize,EditionStandard,EditionNumbered,
			EditionLettered,EditionDelux, wl.Watchers WatchCount
			,NULL IsWatched, ImageCover, ImageFeature, [Description]
	
	from dbo.Book b
	LEFT JOIN (SELECT Count(Id)Watchers, BookId  FROM WatchList WHERE IsDeleted = 0 GROUP By BookId) wl on wl.BookId = b.Id
	LEFT JOIN (SELECT STRING_AGG(Genre, ', ') AS Genre, bg.BookId FROM BookGenre bg 
				LEFT JOIN Genre g on g.Id = bg.GenreId group by bg.BookId) g on g.BookId = b.Id
	LEFT JOIN (SELECT PublishingHouse, Id Pid FROM dbo.Publisher) p on p.Pid = b.PublishingHouseId
	where IsDeleted <>1
	AND IsFeatured = 1

	ORDER BY ReleaseDate ASC, BookTitle;
	
END
