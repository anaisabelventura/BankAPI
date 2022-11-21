namespace BankAPI.WebAPI.Models.Responses
{
    public class RenewResponse
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}
