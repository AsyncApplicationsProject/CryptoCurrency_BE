public class TradeResult
{
    public bool IsSuccess { get; }
    public string Message { get; }

    private TradeResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static TradeResult Success() => new(true, "Success");
    public static TradeResult Failure(string message) => new(false, message);
}
