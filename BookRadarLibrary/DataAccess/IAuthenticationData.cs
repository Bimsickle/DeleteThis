using BookRadarLibrary.Models;
using static BookRadarLibrary.DataAccess.AuthenticationData;

namespace BookRadarLibrary.DataAccess
{
    public interface IAuthenticationData
    {
        Task RegisterIdentityId(UserAccountModel user);
        Task RegisterResfreshToken(RefreshTokenModel token);
        Task RevokeToken(RefreshTokenModel token);
        Task<bool> VerifyAccountExists(AuthenticationUserModel user);
        Task<RefreshTokenModel>? VerifyRefreshToken(RefreshTokenModel token);
    }
}