using BookRadarLibrary.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace BookRadarLibrary.DataAccess;

public class AuthenticationData : IAuthenticationData
{
    private readonly ISqlDataAccess _sql;
    public record EmailConfirmedRecord(bool EmailConfirmed);
    
    public AuthenticationData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    
    public async Task RegisterIdentityId(UserAccountModel user)
    {
         await _sql.SaveData<dynamic>(
            "dbo.spUserAccount_Register",
            new { user.UserId, user.UserEmail  },
            "Default");
    }
    public async Task<bool> VerifyAccountExists(AuthenticationUserModel user)
    {
        var result = await _sql.LoadData<StringIdModel, dynamic>(
            "dbo.spUserAccount_Verify",
            new { UserId = user.IdentityId, user.UserEmail },
            "Default");

        if (result.Count() ==0)
        {
            return false;
        }
        return true;
    }

    public async Task<RefreshTokenModel>? VerifyRefreshToken(RefreshTokenModel token)
    {
        var result = await _sql.LoadData<RefreshTokenModel, dynamic>(
            "dbo.spUserAccount_ValidateToken",
            new { token.UserId, Token = token.RefreshToken, token.Device },
            "Default");

        if (result.Count() == 0)
        {
            return null!;
        }
        return result!.FirstOrDefault()!;
    }
    public async Task RegisterResfreshToken(RefreshTokenModel token)
    {
        await _sql.SaveData<dynamic>(
            "dbo.spUserAccount_CreateRefreshToken",
            new { token.UserId, Token = token.RefreshToken, ExpDate = token.ExprationDateUtc, token.Device },
            "Default");
    }

    public async Task RevokeToken(RefreshTokenModel token)
    {
        await _sql.SaveData<dynamic>(
            "spUserAccount_RevokeToken",
            new { token.UserId, token.Device },
            "Default");
    }
}

/*
 * 
    public async Task<PasswordHashModel?> GetPasswordHash(string userEmail, string? method = "Local")
    {
        var passwordHashModel = await _sql.LoadData<PasswordHashModel, dynamic>(
            "dbo.spUserAccount_GetPasswordHashAndSalt",
            new { UserEmail = userEmail, Method = method },
            "Default");
        if (!passwordHashModel.IsNullOrEmpty() )
        {
            return passwordHashModel.FirstOrDefault();
        }
        else
        {
            return null;
        }
    }

    public async Task<UserAccountModel?> GetUserAccount(string userName)
    {
        var user = await _sql.LoadData<UserAccountModel, dynamic>(
            "dbo.spUserAccount_GetByUserName",
            new { UserName = userName },
            "Default");
        if (!user.IsNullOrEmpty())
        {
            return user.FirstOrDefault();
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<AccountExistsModel>> CheckIfAccountExists(string userName, string userEmail)
    {
        var userExists = await _sql.LoadData<AccountExistsModel, dynamic>(
            "dbo.spUserAccount_CheckIfExists",
            new { UserName = userName, Email = userEmail },
            "Default");
        
        return userExists;
    }

    public async Task<bool> CheckIfEmailVerified(string userName)
    {
        var isVerified = await _sql.LoadData<EmailConfirmedRecord, dynamic>(
            "dbo.spUserAccount_CheckIfEmailConfirmed",
            new { UserName = userName },
            "Default");

        if (isVerified == null)
        {
            return false;
        }
        else
        {
            return isVerified.FirstOrDefault()!.EmailConfirmed;
        }

    }

    public async Task ConfirmEmail(string userName, string userEmail, string method, string token)
    {
        await _sql.SaveData< dynamic>(
            "dbo.spUserAccount_ValidateEmail",
            new { UserName = userName, UserEmail = userEmail, Method= method, Token = token },
            "Default");
    }

    public async Task<UserRegistrationModel?> GetUserRefreshToken(string userEmail)
    {
        var user = await _sql.LoadData<UserRegistrationModel, dynamic>(
            "dbo.spUserAccountRegistration_GetByEmail",
            new { UserEmail = userEmail },
            "Default");
        if (!user.IsNullOrEmpty())
        {
            return user.FirstOrDefault();
        }
        else
        {
            return null;
        }
    }
    public async Task UpdateuserRefreshToken(string userEmail, Guid userAccountId, string refreshToken, DateTime refreshExpiry)
    {
        await _sql.SaveData<dynamic>(
            "dbo.spUserAccountRegistration_UpdateRefreshToken",
            new { UserEmail = userEmail, UserAccountId = userAccountId, RefreshToken = refreshToken, RefreshExpiry = refreshExpiry },
            "Default");
    }
    public async Task RevokeRefreshToken(string userEmail)
    {
        await _sql.SaveData<dynamic>(
            "dbo.spUserAccount_RevokeRefreshToken",
            new { Email = userEmail },
            "Default");
    }

    
 */