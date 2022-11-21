using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace BankAPI.Core.Models
{
    public partial class User 
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Tokens = new HashSet<Token>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime PassWordChangedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
