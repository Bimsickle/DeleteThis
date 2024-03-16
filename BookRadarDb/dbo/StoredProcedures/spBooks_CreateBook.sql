CREATE PROCEDURE [dbo].[spBooks_CreateBook]
    @BookTitle NVARCHAR(255),
    @Author NVARCHAR(255),
    @PublishingHouseId UNIQUEIDENTIFIER,
    @Url NVARCHAR(255) = null,
    @ReleaseDate DATETIME2(7)= NULL,
    @IsReleased BIT = 0,
    @IsFeatured BIT = 0,
    @TotalRunSize INT = 0,
    @EditionStandard INT = 0,
    @EditionNumbered INT = 0,
    @EditionLettered INT = 0,
    @EditionDelux INT = 0,
    @ImageCover NVARCHAR(MAX) = NULL,
    @ImageFeature NVARCHAR(MAX) = NULL,
    @Description NVARCHAR(MAX) = NULL
AS
BEGIN
   
    
    INSERT INTO dbo.Book (
        BookTitle, 
        Author, 
        PublishingHouseId, 
        [Url], 
        ReleaseDate, 
        IsReleased, 
        IsFeatured,
        TotalRunSize,
        EditionStandard,
        EditionNumbered,
        EditionLettered,
        EditionDelux,
        ImageCover,
        ImageFeature,
       [Description]
    )
    VALUES (
        @BookTitle, 
        @Author, 
        @PublishingHouseId, 
        @Url, 
        @ReleaseDate, 
        @IsReleased, 
        @IsFeatured,
        @TotalRunSize,
        @EditionStandard,
        @EditionNumbered,
        @EditionLettered,
        @EditionDelux,
        @ImageCover,
        @ImageFeature,
        @Description
    );

END