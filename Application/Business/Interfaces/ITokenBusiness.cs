using BankAPI.Core.Models;
using BankAPI.Models.Requests;

namespace BankAPI.Application.Business.Interfaces
{
    public interface ITokenBusiness
    {
        Task<(User, string, string, DateTime, DateTime)> Revalidate(int userId, RenewRequest revalidateRequest);

    }
}
