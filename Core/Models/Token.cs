using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BankAPI.Core.Models
{
    public class Token
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Refresh_Token { get; set; }
        public DateTime RefreshToken_expireAt   { get; set; }
        [NotMapped]
        public User User { get; set; }
    }
}
