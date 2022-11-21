namespace BankAPI.WebAPI.Models.Responses
{
    public class TransferResponse
    {
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public static TransferResponse ResponseToTransfer(TransferResponse transferResponse)
        {
            var transfer = new TransferResponse
            {
                Amount = transferResponse.Amount,
                CreatedAt = transferResponse.CreatedAt,
            };
            return transfer;
        }
    }
}
