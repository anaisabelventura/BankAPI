using BankAPI.Models.Requests;
using BankAPI.Core.Models;

namespace BankAPI.Business.Interfaces
{
    public interface IAccountBusiness
    {
        Task<(bool, string, Account?)> Create(AccountRequest accountRequest, int userId);
        Task<List<Account>> GetAccountsByUserId(int userId);
        Task<Account> GetById(int accountId);
        void Update(Account accountToUpdate);
    }
}
