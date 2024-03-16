CREATE PROCEDURE [dbo].[spNews_GetAll]

AS
BEGIN
	SET NOCOUNT ON;
	SELECT n.Id, Title, Content, n.AuthorId [Author], PublishDate, Tags, IsPublished, PreviewImage, BannerImage

	FROM [dbo].[NewsArticle] n
	WHERE n.IsDeleted <>1
	
	ORDER BY PublishDate DESC;
END
