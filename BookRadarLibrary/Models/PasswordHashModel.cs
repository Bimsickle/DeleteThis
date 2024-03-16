using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.Models
{
    public class PasswordHashModel
    {
        public string Password {  get; set; }
        public string? PasswordHash { get; set; }
        public byte[]? Salt { get; set; }
    }
}
