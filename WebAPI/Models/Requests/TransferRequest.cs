using BankAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Requests
{
    public class TransferRequest
    {
        [Required] public int Amount { get; set; }
        [Required] public int SenderAccount { get; set; }
        [Required] public int DestinationAccount { get; set; }

        public static Transfer FromTransferRequestToTransfer(TransferRequest transferRequest)
        {
            var transfer = new Transfer
            {
                Amount = transferRequest.Amount,
                SenderAccountId = transferRequest.SenderAccount,
                DestinationAccountId = transferRequest.DestinationAccount
            };
            return transfer;
        }
    }
}
