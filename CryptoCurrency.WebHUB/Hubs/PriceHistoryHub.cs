using Microsoft.AspNetCore.SignalR;

namespace CryptoCurrency.WebAPI.Hubs
{
    public class PriceHistoryHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}