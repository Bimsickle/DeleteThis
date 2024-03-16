using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.Models
{
    public class GenreModel
    {
        public Guid? Id { get; set; }
        public Guid? GenreId { get; set; }
        public string? Genre { get; set; }
        public Guid? BookId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
