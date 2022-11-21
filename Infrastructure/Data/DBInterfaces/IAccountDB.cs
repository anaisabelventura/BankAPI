using BankAPI.Core.Models;

namespace BankAPI.Data.DBInterfaces
{
    public interface IAccountDB : IBaseDB<Account>
    {
        Task<List<Account>> GetAccountsByUserId(int userId);
        Task<Account> Create(Account accountCreate);
        Task<Account> GetById(int userId);
        Task<Account> Update(Account accountUpdate);
    }
}
