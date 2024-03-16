using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.Models
{
    public class UserBookCollectionModel
    {
        public Guid Id { get; set; }
        public string BookEdition { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string PublishingHouse { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public string ImageCover { get; set; } = string.Empty;
        public string ImageFeature { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<GenreModel>? GenreList { get; set; }
    }
}
