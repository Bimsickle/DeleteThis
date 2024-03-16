using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.Models
{
    public class NewsArticleModel
    {
        public DateTime? CreatedDate { get; set; }
        public Guid? Id { get; set; }
        public string Title {  get; set; }
        public string Content { get; set; }
        public string? Author { get; set; }
        public string? AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public string Tags { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string? BannerImage { get; set; }
        public string? PreviewImage { get; set; }
    }
}
