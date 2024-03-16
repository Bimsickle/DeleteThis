using BookRadarLibrary.DataAccess;
using BookRadarLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using BookRadarApi.MailSettings;
using System.Reflection;
using static Dapper.SqlMapper;
using static BookRadarLibrary.DataAccess.AuthenticationData;

namespace BookRadarApi.Controlers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{    
    private readonly IConfiguration _config;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationData _data;
    private readonly IEmailSender _emailSender;
    private readonly IAccessLogData _accessData;

    public AuthenticationController(IConfiguration config, ILogger<AuthenticationController> logger, IAuthenticationData data, 
                                    IEmailSender emailSender, IAccessLogData accessData)
    {
        _config = config;
        _logger = logger;
        _data = data;
        _emailSender = emailSender;
        _accessData = accessData;
    }

    //public record AuthenticationUser(string? UserEmail, string? Password, string Method = "local");
    //public record AuthenticationUser(string UserEmail, string UserName, string IdentityId, List<string>? Roles);
    public record LoginResponse(string Token, string RefreshToken, DateTime? TokenExpiry,DateTime? RefreshExpiry);

    /*** CONTROLLERS ***/

    [HttpPost("token")] //login
    [AllowAnonymous]
    public async Task <ActionResult<string>> Authenticate([FromBody] AuthenticationUserModel user)
    {
        //var user = await ValidateCredentials(data);

        if (user is null)
        {
            return Unauthorized();
        }
        else
        {
            // verify Account
            bool exists = await _data.VerifyAccountExists(user);
            if (!exists)
            {
                return Unauthorized();
            }
            string token = GenerateToken(user);

            //create & write refresh token
            int refreshValue = 120;
            string refreshMetric = "day";// minute, hour

            // don't change this bit. It is set above ^^
            DateTime refreshTime = new();
            if (refreshMetric == "day")
            {
                refreshTime = DateTime.Now.AddDays(refreshValue);
            }
            else if (refreshMetric == "hour")
            {
                refreshTime = DateTime.Now.AddHours(refreshValue);
            }
            else
            {
                refreshTime = DateTime.Now.AddMinutes(refreshValue);
            }

            var refreshToken = GenerateRefreshToken();
            RefreshTokenModel refresh = new RefreshTokenModel { UserId = user.IdentityId , RefreshToken = refreshToken, ExprationDateUtc = refreshTime};
            await _data.RegisterResfreshToken(refresh);
            var expTime = GetTokenExpirationDate(token);
            var loginResponse = new LoginResponse(token, refreshToken, expTime, refreshTime);

            if (!user.Roles!.Contains("Demigod"))
            {
                // Log login - for reporting :)
                AccessLogModel accessLog = new AccessLogModel { UserId = user.IdentityId, UserName = user.UserName, EventAction = "Login" };
                await _accessData.WriteAccessLogEntry(accessLog);
            }

            return Ok(loginResponse);
        }        
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserAccountModel?>> RegisterUserAccount(UserAccountModel registration)
    {
        // check if exists
        if ( !registration.UserEmail.IsNullOrEmpty() || !registration.UserId.IsNullOrEmpty())
        {
            await _data.RegisterIdentityId(registration);
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }


    [HttpPost("Refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Refresh()
    {
        if (string.IsNullOrEmpty(Request.Headers["RefreshToken"]))
        {
            return BadRequest(); //500
        }
        string? tokenHead = Request.Headers["Authorization"]!;
        string? token = tokenHead?.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest();
        }
        string refreshToken = Request.Headers["RefreshToken"]!.FirstOrDefault();
        var model = new RefreshModel
        {
            Token = token,
            RefreshToken = refreshToken
        };
        var principal = GetPrincipalFromExpiredToken(model.Token);
        if (principal?.Identity?.Name is null)
        {
            return BadRequest(); //500
        }

        var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var userName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var roles = ExtractClaims(principal, ClaimTypes.Role);

        //refresh tokens are duplicating....
        //string[] refreshTokens = model.RefreshToken.Split(',');
        RefreshTokenModel refresh = new RefreshTokenModel { UserId = userId!, RefreshToken = model.RefreshToken };// refreshTokens.FirstOrDefault()?.Trim()};

        RefreshTokenModel validated = await _data.VerifyRefreshToken(refresh);
        if (validated == null)
        {
            return Unauthorized(); //401
        }
        // check token time
        var tokenExpTime = GetTokenExpirationDate(token);
        if (tokenExpTime> DateTime.UtcNow.AddSeconds(30))
        {
            //token is valid send back
            var response = new LoginResponse(token, model.RefreshToken, tokenExpTime, validated.ExprationDateUtc);
            return Ok(response); //200
        }

        AuthenticationUserModel user = new AuthenticationUserModel { UserEmail = userEmail! , UserName = userName! , IdentityId = userId! , Roles = roles};
        var newToken = GenerateToken(user!);
        var expTime = GetTokenExpirationDate(newToken);
        var loginResponse = new LoginResponse(newToken, model.RefreshToken, expTime, validated.ExprationDateUtc);
        return Ok(loginResponse); //200
    }

    [HttpDelete("Revoke")]
    public async Task<IActionResult> Revoke()
    {
        _logger.LogInformation("Revoke Token Called");
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        RefreshTokenModel refreshTokenModel = new RefreshTokenModel { UserId = userId };
        // Revoke refresh token
        await _data.RevokeToken(refreshTokenModel)!;
        return Ok();
    }


    /**** TOKENS & AUTHORISATION ****/
    private string GenerateToken(AuthenticationUserModel user)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
                _config.GetValue<string>("Authentication:SecretKey") ?? string.Empty));

        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.IdentityId.ToString()));
        claims.Add(new(ClaimTypes.Name, user.UserName));
        claims.Add(new(ClaimTypes.Email, user.UserEmail));
        
        //List<string>? roles = user.Roles!.Split(",").ToList();// min "user" role
        try
        {
            if (user.Roles is not null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"No Roles {ex}");
        }

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            //DateTime.Now.AddSeconds(30),
            DateTime.UtcNow.AddMinutes(45),
            signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
   

    private DateTime GetTokenExpirationDate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken != null)
        {
            return jwtToken.ValidTo;
        }
        else
        {
            throw new ArgumentException("Invalid token or token does not contain expiration date.");
        }
    }

    /*** Refresh Token ***/
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        // Totally from youtube, this is a mystery = Anj :)

        var secret = _config.GetValue<string>("Authentication:SecretKey");

        var validation = new TokenValidationParameters
        {
            ValidIssuer = _config.GetValue<string>("Authentication:Issuer"),
            ValidAudience = _config.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = false
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private List<string> ExtractClaims(ClaimsPrincipal userPrincipal, string claimType)
    {
        // Retrieve all claims of the specified type from the user's principal
        var claims = userPrincipal.Claims.Where(c => c.Type == claimType).ToList();

        // Extract the values of the claims and add them to a list
        var claimValues = new List<string>();
        foreach (var claim in claims)
        {
            claimValues.Add(claim.Value);
        }

        return claimValues;
    }

    ///**** User Registration ***/
    //private string GenerateConfirmationToken(UserRegistrationModel user)
    //{
    //    var secretKey = new SymmetricSecurityKey(
    //        Encoding.ASCII.GetBytes(
    //            _config.GetValue<string>("Authentication:SecretKey") ?? string.Empty));
    //    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    //    List<Claim> claims = new();
    //    claims.Add(new(ClaimTypes.Name, user.UserName));
    //    claims.Add(new(ClaimTypes.Email, user.UserEmail));
    //    claims.Add(new Claim(ClaimTypes.Role, user.RegistrationMethod)); // registration method

    //    var token = new JwtSecurityToken(
    //        _config.GetValue<string>("Authentication:Issuer"),
    //        _config.GetValue<string>("Authentication:Audience"),
    //        claims,
    //        DateTime.Now,
    //        DateTime.Now.AddMinutes(20),
    //        signinCredentials);

    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}


    /*** EMAIL ***/
    private void SendEmailConfirmation(string receiver, string userName, string token)
    {
        var subject = "Welcome to Book Radar";
        string path = _config.GetValue<string>("MailSettings:RedirectUrl")!;

        EmailModel eml = new();
        eml.GenerateWelcomeEmail(path, userName, token);

        _emailSender.SendEmailsAsync(receiver, subject, eml.WelcomePlainEmail, eml.WelcomeHtmlEmail);        
    }
}
