using BankAPI.Application.Business.Interfaces;
using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Models.Requests;
using System.IdentityModel.Tokens.Jwt;

namespace BankAPI.Application.Business
{
    public class TokenBusiness : ITokenBusiness
    {
        private ITokenRepository _tokenDb;
        private IUserDB _usersDb;
        private IJwt _jwtAuth;
        private IConfiguration _config;

        public TokenBusiness(ITokenRepository tokenDb, IUserDB usersDb, IJwt jwtAuth, IConfiguration config)
        {
            _tokenDb = tokenDb;
            _usersDb = usersDb;
            _jwtAuth = jwtAuth;
            _config = config;
        }

        public async Task<(User, string, string, DateTime, DateTime)> Revalidate(int userId, RenewRequest revalidateRequest)
        {
            //Validate User Login
            User user = await _usersDb.GetById(userId);

            var token = await _tokenDb.GetTokensByRefreshToken(revalidateRequest.RefreshToken);
            if (token == null || user == null)
            {
                throw new ArgumentException("User not the same!");
            }

            if (!(user.Id.Equals(token.UserID)))
            {
                throw new ArgumentException("Token not valid!");
            }

            JwtSecurityToken accessToken = _jwtAuth.CreateJwtToken(user);

            var access = new JwtSecurityTokenHandler().WriteToken(accessToken);

            string refreshToken = _jwtAuth.CreateUserRefreshToken();

            token.UserID = user.Id;
            token.Refresh_Token = refreshToken;
            token.RefreshToken_expireAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresMin"]));

            //Persist Token
            await _tokenDb.Update(token);

            return (user, access, refreshToken, accessToken.ValidTo, token.RefreshToken_expireAt);
        }

    }
}
