namespace CryptoCurrency.Services.Interfaces
{
    public interface ITradeService
    {
        public Task<TradeResult> Buy(string symbol, int amount, string userId);
        public Task<TradeResult> Sell(string symbol, int amount, string userId);
    }
}
