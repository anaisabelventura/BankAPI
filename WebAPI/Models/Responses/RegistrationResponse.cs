using BankAPI.Core.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Responses
{
    public class RegistrationResponse
    {
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string FullName { get; set; }
       // [Required, DefaultValue("string")] public DateTime PasswordChangedAt { get; set; }
        [Required] public string Username { get; set; }

        public static RegistrationResponse ToCreateUserResponse(User user)
        {
            var registrationResponse = new RegistrationResponse
            {
                UserId = user.Id,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                FullName = user.FullName,
               // PasswordChangedAt = DateTime.UtcNow,
                Username = user.UserName,
            };
            return registrationResponse;
        }

    }
}
