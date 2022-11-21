using BankAPI.Core.Models;
using BankAPI.Models.Requests;

namespace BankAPI.Business.Interfaces
{
    public interface ISessionBusiness
    {
        Task<(User, Session)> Login(LoginRequest userRequest);
        Task<Session> CreateSession(Session session);
        Task<(User, Session)> Renew(string token, string sessionId);
        Task<(User, Session)> Logout(Guid id, int userId);
        Task<bool> CheckSession(string sessionId);
    }
}
