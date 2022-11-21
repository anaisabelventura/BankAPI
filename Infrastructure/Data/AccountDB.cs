using BankAPI.Data.DBInterfaces;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data
{
    public class AccountDB : BaseDB<Account>, IAccountDB, IBaseDB<Account>
    {
        public AccountDB(DbContextOptions<postgresContext> options) : base(options)
        {
        }
        public async override Task<Account> Create(Account account)
        {
            await _db.AddAsync(account);
            await _db.SaveChangesAsync();
            return account;
        }
        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            return await _db.Account.Where(a => a.UserId == userId).ToListAsync();

        }
        public async Task<Account> GetById(int accountId)
        {
            return await _db.Account.FirstOrDefaultAsync(a => a.ID == accountId);
        }

        public async Task<Account> Update(Account accountUpdate)
        {
            _db.Account.Update(accountUpdate);
            await _db.SaveChangesAsync();
            return accountUpdate;
        }
    }
}
