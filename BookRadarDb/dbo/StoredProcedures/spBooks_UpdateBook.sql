create procedure [dbo].[spBooks_UpdateBook]
	@Id UNIQUEIDENTIFIER,
    @BookTitle NVARCHAR(255),
    @Author NVARCHAR(255),
    @PublishingHouseId UNIQUEIDENTIFIER,
    @Url NVARCHAR(255)= NULL,
    @ReleaseDate DATETIME2(7) = NULL,
    @IsReleased BIT,
    @IsFeatured BIT,
    @TotalRunSize INT,
    @EditionStandard INT,
    @EditionNumbered INT,
    @EditionLettered INT,
    @EditionDelux INT,
    @ImageCover NVARCHAR(MAX) = NULL,
    @ImageFeature NVARCHAR(MAX) = NULL,
    @Description NVARCHAR(MAX) = NULL
as
begin
    
	update dbo.Book
	set BookTitle = @BookTitle,
        Author = @Author,
        PublishingHouseId = @PublishingHouseId,
        Url = @Url,
        ReleaseDate = @ReleaseDate,
        IsReleased = @IsReleased,
        IsFeatured = @IsFeatured,
        TotalRunSize = @TotalRunSize,
        EditionStandard = @EditionStandard,
        EditionNumbered = @EditionNumbered,
        EditionLettered = @EditionLettered,
        EditionDelux = @EditionDelux,
        ImageCover = @ImageCover,
        ImageFeature = @ImageFeature,
        [Description] = @Description
	where
		Id = @Id
end