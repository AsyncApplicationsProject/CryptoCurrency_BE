namespace CryptoCurrency.Services.Interfaces
{
    public interface ITradeService
    {
        public Task<bool> Buy(string symbol, int amount, string token);
        public Task<bool> Sell(string symbol, int amount, string token);
    }
}
