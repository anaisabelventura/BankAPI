using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Core.Models
{
    public class Transfer 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SenderAccountId { get; set; }
        public int DestinationAccountId { get; set; }
 
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
