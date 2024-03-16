
namespace BookRadarLibrary.Models;

public class WatchListModel
{        
    public Guid? Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid UserAccountId { get; set; }
    public Guid BookId { get; set; }
    public BookModel? Book { get; set; }
    public bool IsDeleted { get; set; }
}
