using BankAPI.Business.Interfaces;
using System.Transactions;
using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;
using BankAPI.Application.Business.Interfaces;
using BankAPI.Application.Business.Enums;
using BankAPI.Data;
using BankAPI.Models.Requests;
using System.Security.Authentication;
using BankAPI.Infrastructure.Data.DBInterfaces;

namespace BankAPI.Business
{
    public class TransferBusiness : ITransferBusiness
    {
        protected ITransferDB _transferDB;
        protected IAccountDB _accountDB;
      

        public TransferBusiness(ITransferDB transferDB, IAccountDB accountDB, ILogger<TransferBusiness> logger)
        {
            _transferDB = transferDB;
            _accountDB  = accountDB;
        }
        public async Task<string> CreateTransfer(TransferRequest transferRequest, int userId)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Convert TransferRequest to Transfer
                var transfer = TransferRequest.FromTransferRequestToTransfer(transferRequest);
                var fromAccount = await _accountDB.GetById(transfer.SenderAccountId);
                var toAccount = await _accountDB.GetById(transfer.DestinationAccountId);

                //switch
                //Validates
                if (fromAccount.UserId != userId) throw new AuthenticationException("User not owner account");
                if (fromAccount is null || toAccount is null) throw new ArgumentException("Accounts not valid");
                if (fromAccount.Balance < transfer.Amount) throw new ArgumentException("Insufficient funds from your account");
                if (fromAccount.Currency != toAccount.Currency) throw new ArgumentException("Currency isn't the same");

                await _transferDB.Create(transfer);
                //throw new ArgumentException("Transferencia terminada valida");

                var amount = transfer.Amount;

                //Debit update account
                toAccount.Balance += amount;
                await _accountDB.Update(toAccount);


                //Credit update account
                amount = amount * -1;
                fromAccount.Balance += amount;
                await _accountDB.Update(fromAccount);

                
                transactionScope.Complete();

                return "Transfer completed";
            }

        }
    }
}
