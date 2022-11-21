using BankAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Requests
{
    public class RegistrationRequest
    {
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string UserName { get; set; }
        [Required, MinLength(6) ]public string Password { get; set; }
        [Required, MinLength(10)] public string FullName { get; set; }

        public static User RequestToUser(RegistrationRequest request)
        {
            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName
            };
            return user;
        }
    }
}
