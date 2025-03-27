using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class TradeHub : Hub
{
    private readonly ITradeService _tradeService;

    public TradeHub(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;
        if (userId == null)
        {
            await Clients.Caller.SendAsync("TradeResponse", "ClientHub is not authenticated.");
            return;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Buy(string symbol, int amount)
    {
        var userId = Context.User?.Identity?.Name;
        if (userId == null)
        {
            await Clients.Caller.SendAsync("TradeResponse", "ClientHub is not authenticated.");
            return;
        }
        var result = await _tradeService.Buy(symbol, amount, userId);
        await Clients.Caller.SendAsync("TradeResponse", result.Message);
    }

    public async Task Sell(string symbol, int amount)
    {
        var userId = Context.User?.Identity?.Name;
        if (userId == null)
        {
            await Clients.Caller.SendAsync("TradeResponse", "ClientHub is not authenticated.");
            return;
        }
        var result = await _tradeService.Sell(symbol, amount, userId);
        await Clients.Caller.SendAsync("TradeResponse", result.Message);
    }
}
