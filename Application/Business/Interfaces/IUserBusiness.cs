using BankAPI.Core.Models;
using BankAPI.Models.Requests;
using BankAPI.Models.Responses;


namespace BankAPI.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<RegistrationResponse> Create(RegistrationRequest userRequest);
        Task<(User, string, string, DateTime, DateTime)> Login(LoginRequest userRequest);

    }
}
