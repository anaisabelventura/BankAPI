using BankAPI.Core.Models;
using BankAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BankAPI.Infrastructure.Authentication.Interface
{
    public interface IJwt
    {
        string CreateUserRefreshToken();
        JwtSecurityToken CreateJwtToken(User user);
        string GetClaim(string authToken, string claimName);
        bool TokenIsValid(string authToken);
    }
}
