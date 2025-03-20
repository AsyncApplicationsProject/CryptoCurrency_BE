using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CryptoCurrency.WebAPI.Hubs
{
    public class PriceHistoryHub : Hub
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public PriceHistoryHub(IPriceHistoryService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        public async Task SendPriceUpdate()
        {
            await Clients.All.SendAsync("ReceivePriceUpdate");
        }
    }
}