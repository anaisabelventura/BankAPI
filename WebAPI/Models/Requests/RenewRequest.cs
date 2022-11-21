using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Requests
{
    public class RenewRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
