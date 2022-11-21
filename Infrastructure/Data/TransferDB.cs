using BankAPI.Core.Models;
using BankAPI.Data.DBInterfaces;
using BankAPI.Infrastructure.Data;
using BankAPI.Infrastructure.Data.DBInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data
{
    public class TransferDB : BaseDB<Transfer>, ITransferDB, IBaseDB<Transfer>
    {
        public TransferDB(DbContextOptions<postgresContext> options) : base(options)
        {
        }
        public async override Task<Transfer> Create(Transfer transfer)
        {
            await _db.AddAsync(transfer);
            await _db.SaveChangesAsync();
            return transfer;
        }

        public async Task<Transfer> Update(Transfer transferUpdate)
        {
            _db.Transfer.Update(transferUpdate);
            await _db.SaveChangesAsync();
            return transferUpdate;
        }

        public async Task<Transfer> GetById(int transferId)
        {
            return await _db.Transfer.FirstOrDefaultAsync(a => a.Id == transferId);
        }
    }
}
