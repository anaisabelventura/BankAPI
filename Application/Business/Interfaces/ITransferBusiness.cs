
using BankAPI.Application.Business.Enums;
using BankAPI.Core.Models;
using BankAPI.Models.Requests;

namespace BankAPI.Business.Interfaces
{
    public interface ITransferBusiness
    {
        Task<string> CreateTransfer(TransferRequest transferRequest, int userId);
    }
}
