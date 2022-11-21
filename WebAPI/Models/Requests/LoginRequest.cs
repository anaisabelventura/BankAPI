using BankAPI.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Requests
{
    public class LoginRequest
    {
        [Required, MaxLength(20), MinLength(5)] public string Username { get; set; }
        [Required, MinLength(6), MaxLength(20)] public string Password { get; set; }

        public static User UsertoLoginRequest(LoginRequest request)
        {
            var user = new User()
            {
                UserName = request.Username,
                Password = request.Password
            };
            return user;
        }
    }
}
