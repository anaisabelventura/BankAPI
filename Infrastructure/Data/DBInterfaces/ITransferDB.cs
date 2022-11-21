using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;

namespace BankAPI.Infrastructure.Data.DBInterfaces
{
    public interface ITransferDB  : IBaseDB<Transfer>
    {
        Task<Transfer> Create(Transfer transferCreate);
        Task<Transfer> Update(Transfer transferUpdate);
        Task<Transfer> GetById(int userId);
    }
}
