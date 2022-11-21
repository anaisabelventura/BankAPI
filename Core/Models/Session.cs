namespace BankAPI.Core.Models
{
    public class Session 
    {
        public Guid ID { get; set; }
        public User? User { get; set; }
        public int UserID { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenValidTo { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? AccessTokenValidTo { get; set; }
        public bool IsActive { get; set; }
    }
}
