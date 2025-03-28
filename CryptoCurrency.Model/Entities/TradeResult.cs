public class TradeResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public decimal? UserBalance { get; set; }

    private TradeResult(bool isSuccess, string message, decimal? userBalance)
    {
        IsSuccess = isSuccess;
        Message = message;
        UserBalance = userBalance;
    }

    public static TradeResult Success(decimal userBalance) => new(true, "Success", userBalance);
    public static TradeResult Failure(string message) => new(false, message, null);
}
