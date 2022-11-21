using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;
using Token = BankAPI.Core.Models.Token;

namespace BankAPI.Infrastructure.Data.DBInterfaces
{
    public interface ITokenDB : IBaseDB<Token>
    {
        Task<Token> Create(Token Token);
        Task<Token> GetTokensByRefreshToken(string refreshToken);
        //Task<Token> GetByToken(string token);
    }
}
