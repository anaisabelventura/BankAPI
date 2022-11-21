namespace BankAPI.Application.Business.Enums
{
    public enum TransferCodes
    {
        InvalidUser,
        DifferentCurrency,
        InsufficientFunds,
        DestinationEqualtoSender,
        Successfull,
        Failed,
    }
}