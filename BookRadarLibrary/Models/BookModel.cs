namespace BookRadarLibrary.Models;

public class BookModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime CreatedDate {  get; set; } = DateTime.UtcNow;
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string PublishingHouse { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid? PublishingHouseId { get; set; } 
    public string? Url { get; set; } = string.Empty;
	public DateTime? ReleaseDate { get; set; } = new DateTime();
	public bool IsReleased { get; set; } = false;
	public bool IsFeatured { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
	public int TotalRunSize { get; set; } = 0;
	public int EditionStandard { get; set; } = 0;
    public int EditionNumbered { get; set; } = 0;
    public int EditionLettered { get; set; } = 0;
    public int EditionDelux { get; set; } = 0;
	public int WatchCount { get; set; } = 0;
    public bool IsWatched { get; set; } = false;
    public string? ImageCover { get; set; }
    public string? ImageFeature { get; set; }


    // Genre
    public List<GenreModel>? GenreList { get; set; }

}
