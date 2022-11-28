using BankAPI.Core.Enums;
using BankAPI.Models.Responses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Core.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        [MinLength(3), DefaultValue("EUR")]
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }
        public User User { get; set; }
        //public List<Movement> movements { get; set; }
    }
}