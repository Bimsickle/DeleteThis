using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.Models;

public class AccessLogModel
{ 
    public DateTime? CreatedDate { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName {get; set;} = string.Empty;
    public string EventAction {get; set;} = string.Empty;
    public string? IdType {get; set;} // e.g. Book, News, etc
    public Guid? TypeId {get; set;} // id for above
    public string? Description { get; set;} = string.Empty;// e.g book title, news article title etc
}
