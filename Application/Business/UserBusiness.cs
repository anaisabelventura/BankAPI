using BankAPI.Business.Interfaces;
using BankAPI.Data.DBInterfaces;
using BankAPI.Models.Requests;
using Microsoft.AspNetCore.Identity;
using static BankAPI.Models.Responses.RegistrationResponse;
using BankAPI.Core.Models;
using BankAPI.Models.Responses;
using BankAPI.Infrastructure.Authentication.Interface;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;

namespace BankAPI.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserDB _userDb;
        private readonly ITokenRepository _tokenDb;
        private readonly IJwt _jwtAuth;
        private readonly IConfiguration _config;
        public UserBusiness(IUserDB usersDb, IJwt jwtAuth, ITokenRepository tokenDb, IConfiguration _configuration)
        {
            _userDb = usersDb;
            _jwtAuth = jwtAuth;
            _tokenDb = tokenDb;
            _config = _configuration;
        }
        public async Task<RegistrationResponse> Create(RegistrationRequest userRequest)
        {
            try
            {
                if (await _userDb.GetByUsername(userRequest.UserName) is not null)
                {
                    throw new ArgumentException("Username cannot be repeated");
                }
                //UserRequest

                User user = RegistrationRequest.RequestToUser(userRequest);

                //Hash password
                var passwordHasher = new PasswordHasher<RegistrationRequest>();
                string passwordHash = passwordHasher.HashPassword(userRequest, userRequest.Password);

                //Persist User
                var CreatedUser = await _userDb.Create(user);
                //Convert user to UserResponse
                var createUserResponse = RegistrationResponse.ToCreateUserResponse(CreatedUser);
                return createUserResponse;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }

     
        }

        public async Task<(User, string, string, DateTime, DateTime)> Login(LoginRequest userRequest)
        {
            //Validate User Login
            User user = await _userDb.GetByUsername(userRequest.Username);

            JwtSecurityToken accessToken = _jwtAuth.CreateJwtToken(user);

            var access = new JwtSecurityTokenHandler().WriteToken(accessToken);

            string refreshToken = _jwtAuth.CreateUserRefreshToken();

            Token token = new Token();

            token.UserID = user.Id;
            token.Refresh_Token = refreshToken;
            token.RefreshToken_expireAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresMin"]));


            //Persist Token
            await _tokenDb.Create(token);
            if (user is null) { throw new AuthenticationException("User not found"); }

            var passwordHasher = new PasswordHasher<LoginRequest>();
            var result = passwordHasher.VerifyHashedPassword(userRequest, user.Password, userRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthenticationException("authentication failed");
            }
            return (user, access, refreshToken, accessToken.ValidTo, token.RefreshToken_expireAt);
        }

    }
}