using System;
using BankAPI.Business.Interfaces;
using BankAPI.Models.Requests;
using BankAPI.Data;
using BankAPI.Data.DBInterfaces;
using BankAPI.Core.Models;
using BankAPI.Models.Responses;

namespace BankAPI.Business
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IAccountDB _accountDb;
        public AccountBusiness(IAccountDB accountsDb)
        {
            _accountDb = accountsDb;
        }
        public async Task<(bool, string, Account?)> Create(AccountRequest accountRequest, int userId)
        {
            try
            {
                //Validate amount
                if (accountRequest.Amount < 0)
                    return (false, "Amount not valid", null);
                //Validate currency
                /*if (!Enum.IsDefined(accountRequest.Currency))
                    return (false, "Currency not valid", null);*/

                var account = AccountRequest.FromUserRequestToAccount(accountRequest, userId);

                var createdAccount = await _accountDb.Create(account);
                return (true, "Account created", createdAccount);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
        }

        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            if (await _accountDb.GetAccountsByUserId(userId) is not null)
            {
                return await _accountDb.GetAccountsByUserId(userId);
            }
            throw new ArgumentException("Account not found");
        }

        public async Task<Account> GetById(int accountId)
        {
            if (await _accountDb.GetById(accountId) is not null)
            {
                return await _accountDb.GetById(accountId);
            }
            throw new ArgumentException("Account not found");
        }

        public void Update(Account accountToUpdate)
        {
            _accountDb.Update(accountToUpdate);
        }
    }
}