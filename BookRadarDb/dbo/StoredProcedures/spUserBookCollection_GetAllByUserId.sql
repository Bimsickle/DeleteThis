CREATE PROCEDURE [dbo].[spUserBookCollection_GetAllByUserId]
	@UserAccountId NVARCHAR(450)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	ubc.Id, ubc.BookEdition, b.Id [BookId], BookTitle, Author, p.PublishingHouse, [Url], ReleaseDate, ImageCover, ImageFeature, b.[Description]
	FROM [dbo].[UserBookCollection] ubc
	LEFT JOIN Book b on b.Id = ubc.BookId
	LEFT JOIN (SELECT PublishingHouse, Id Pid FROM dbo.Publisher) p on p.Pid = b.PublishingHouseId
	
	WHERE ubc.UserAccountId = @UserAccountId
	AND ubc.IsDeleted <>1;
END
