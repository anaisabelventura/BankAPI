using BankAPI.Core.Models;
using BankAPI.Data;
using BankAPI.Data.DBInterfaces;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Infrastructure.Data.DBInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Data
{
    public class TokenRepository : BaseDB<Token>, ITokenRepository, IBaseDB<Token>
    {
        public TokenRepository(DbContextOptions<postgresContext> options) : base(options)
        {
        }


        public async override Task<Token> Create(Token token)
        {
            await _db.AddAsync(token);
            await _db.SaveChangesAsync();
            return token;
        }

        public async Task<Token> GetTokensByRefreshToken(string refreshToken)
        {
            return await _db.Token.FirstOrDefaultAsync(a => a.Refresh_Token == refreshToken);
        }

    }
}
