CREATE PROCEDURE [dbo].[spNewsArticle_EditPost]
	@Id UNIQUEIDENTIFIER,
	@Title nvarchar(255),
	@Content nvarchar(MAX),
	@AuthorId NVARCHAR(450),
	@PublishDate DATE ,
	@Tags NVARCHAR(255) = '#News',
	@IsPublished BIT,
	@PreviewImage NVARCHAR(MAX) = NULL,
	@BannerImage NVARCHAR(MAX) = NULL,
	@IsDeleted BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo]. [NewsArticle]
	SET Title = @Title, 
	Content= @Content, 
	AuthorId= @AuthorId, 
	PublishDate= @PublishDate, 
	Tags= @Tags, 
	IsPublished= @IsPublished, 
	PreviewImage= @PreviewImage, 
	BannerImage = @BannerImage,
	IsDeleted = @IsDeleted

	WHERE Id = @Id;

END
