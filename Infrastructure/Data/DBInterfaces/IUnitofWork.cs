using BankAPI.Data.DBInterfaces;
using System.Data;

namespace BankAPI.Infrastructure.Data.DBInterfaces
{
    public interface IUnitofWork
    {
        IUserDB UserRepository { get; }
        IAccountDB AccountRepository { get; }
        ITransferDB TransferRepository { get; }
        IDbTransaction Begin();
        void Commit();
        void Rollback();
    }
}
