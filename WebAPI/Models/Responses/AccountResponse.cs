using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using BankAPI.Core.Enums;
using BankAPI.Core.Models;

namespace BankAPI.Models.Responses
{
    public class AccountResponse
    {
        public decimal Balance { get; set; }
        [DefaultValue("String")]  public DateTime CreatedAt { get; set; }
        [Required, MinLength(3), DefaultValue("EUR")] public Currency Currency { get; set; }
        public int Id { get; set; }

        public static AccountResponse ToAcountResponse(Account account)
        {
            var accountResponse = new AccountResponse
            {
                Id = account.ID,
                Balance = account.Balance,
                Currency = account.Currency,
                CreatedAt = account.CreatedAt
            };
            return accountResponse;

        }
        public static List<AccountResponse> FromListAccountsUser(List<Account> accounts)
        {
            var accountResponseList = new List<AccountResponse>();
            foreach (var account in accounts)
            {
                var accountResponse = AccountResponse.ToAcountResponse(account);
                accountResponseList.Add(accountResponse);
            }
            return accountResponseList;
        }
    }
}