

namespace BookRadarLibrary.Models;

public class AuthenticationUserModel
{
    public string UserEmail { get; set; } = string.Empty;
    public  string UserName {get;set;} = string.Empty;
    public  string IdentityId {get;set;} = string.Empty;
    public List<string>? Roles { get; set; }
}
