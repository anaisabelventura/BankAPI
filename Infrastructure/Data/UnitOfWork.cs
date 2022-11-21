using BankAPI.Data;
using BankAPI.Data.DBInterfaces;
using BankAPI.Infrastructure.Data.DBInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;

namespace BankAPI.Infrastructure.Data
{
    public class UnitOfWork : IUnitofWork, IDisposable
    {

        public IUserDB UserRepository { get; }
        public IAccountDB AccountRepository { get; }
        public ITransferDB TransferRepository { get; }

        private readonly IDbTransaction _dbTransaction;

        private readonly IDistributedCache _cache;
     
        public UnitOfWork(IUserDB usersDb, IAccountDB accountsDb, ITransferDB transfersDb, IDbTransaction dbTransaction)
        {
            UserRepository = usersDb;
            AccountRepository = accountsDb;
            TransferRepository = transfersDb;
            _dbTransaction = dbTransaction;
        }

        public IDbTransaction Begin()
        {
            try
            {
                return _dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                return _dbTransaction;
            }
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                //Dispose();
                //_dbTransaction.Connection.BeginTransaction();

            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _dbTransaction.Connection?.Close();
                _dbTransaction.Connection?.Dispose();
                _dbTransaction.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
            Dispose();
        }

        ~UnitOfWork()
        {
            Dispose();
        }
    }

}


