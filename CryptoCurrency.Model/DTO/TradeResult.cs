public class TradeResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public decimal? UserBalance { get; set; }
    public string? CryptoSymbol { get; set; }
    public int? CryptoAmount { get; set; }

    private TradeResult(bool isSuccess, string message, decimal? userBalance, string? cryptoSymbol, int? cryptoAmount)
    {
        IsSuccess = isSuccess;
        Message = message;
        UserBalance = userBalance;
        CryptoSymbol = cryptoSymbol;
        CryptoAmount = cryptoAmount;
    }

    public static TradeResult Success(decimal userBalance, string cryptoSymbol, int cryptoAmount) => new(true, "Success", userBalance, cryptoSymbol, cryptoAmount);
    public static TradeResult Failure(string message) => new(false, message, null, null, null);
}
