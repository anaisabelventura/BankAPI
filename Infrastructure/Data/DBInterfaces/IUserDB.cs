using BankAPI.Core.Models;

namespace BankAPI.Data.DBInterfaces
{
    public interface IUserDB : IBaseDB<User>
    {
        Task<User> Create(User user);
        Task<User> GetByUsername(string userName);
        Task<User> GetById(int userId);
    }
}
