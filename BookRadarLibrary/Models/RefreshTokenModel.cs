namespace BookRadarLibrary.Models;

public class RefreshTokenModel
{
    public DateTime? CreatedDate {  get; set; }
    public string UserId {  get; set; } = string.Empty;
    public string Device { get; set; } = "Primary";
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExprationDateUtc { get; set; } = DateTime.UtcNow;
   
}
