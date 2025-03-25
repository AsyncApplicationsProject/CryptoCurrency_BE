using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CryptoCurrency.WebAPI.Hubs
{
    public class TradeHub : Hub
    {
        private readonly ITradeService _tradeService;
        public TradeHub(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        //public async Task Buy(string symbol, int amount, string token)
        //{
        //    var userId = GetUserIdFromToken(token);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return "Invalid token or user not found.";
        //    }

        //}
    }
}
