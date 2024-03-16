CREATE PROCEDURE [dbo].[spNews_GetById]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	n.CreatedDate, n.Id, Title, Content, n.AuthorId, n.AuthorId [Author], PublishDate, Tags, IsPublished, PreviewImage, BannerImage, n.IsDeleted

	FROM [dbo].[NewsArticle] n
	WHERE  n.Id = @Id;
END
