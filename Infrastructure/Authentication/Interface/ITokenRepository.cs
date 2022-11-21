
using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;

namespace BankAPI.Infrastructure.Authentication.Interface
{
   
        public interface ITokenRepository : IBaseDB<Token>
        {
            Task<Token> Create(Token Token);
            Task<Token> GetTokensByRefreshToken(string refreshToken);
            //Task<Token> GetByToken(string token);
        }
    
}
