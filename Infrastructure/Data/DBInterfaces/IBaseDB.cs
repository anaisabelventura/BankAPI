using BankAPI.Core.Models;

namespace BankAPI.Data.DBInterfaces
{
    public interface IBaseDB<T> 
    {
        Task<T> Create(T entity);
        Task<T> Update(T entityToUpdate);
    }
}
