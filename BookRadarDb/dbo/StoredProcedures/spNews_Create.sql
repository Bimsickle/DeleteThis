CREATE PROCEDURE [dbo].[spNews_Create]
	@Title nvarchar(255),
	@Content nvarchar(MAX),
	@AuthorId NVARCHAR(450),
	@PublishDate DATE = GETUTCDATE,
	@Tags NVARCHAR(255) = '#News',
	@IsPublished BIT,
	@PreviewImage NVARCHAR(MAX) = NULL,
	@BannerImage NVARCHAR(MAX) = NULL
AS
BEGIN
	SET NOCOUNT On;

	INSERT INTO [dbo].[NewsArticle]
	(Title, Content, AuthorId, PublishDate, Tags, IsPublished, PreviewImage, BannerImage)
	VALUES
	(@Title, @Content, @AuthorId, @PublishDate, @Tags, @IsPublished, @PreviewImage, @BannerImage)
END