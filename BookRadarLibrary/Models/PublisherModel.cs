
namespace BookRadarLibrary.Models;

public class PublisherModel
{
    public Guid? Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string PublishingHouse { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
