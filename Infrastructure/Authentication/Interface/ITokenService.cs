using BankAPI.Core.Models;
using BankAPI.Models.Requests;
using System.IdentityModel.Tokens.Jwt;

namespace BankAPI.Infrastructure.Authentication.Interface
{
    public interface QITokenService
    {
        Task<(User, string, string, DateTime, DateTime)> Revalidate(int userId, RenewRequest renewRequest);
    }
}
