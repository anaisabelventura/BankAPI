using BankAPI.Core.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BankAPI.Models.Responses
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public RegistrationResponse User { get; set; }

        public LoginResponse(User user, string acess, string refresh, DateTime dateAcess, DateTime dateRefresh)
        {
            AccessToken = acess;
            AccessTokenExpiresAt = dateAcess;

            RefreshToken = refresh;
            RefreshTokenExpiresAt = dateRefresh;

            User = RegistrationResponse.ToCreateUserResponse(user);

        }

    }
}
