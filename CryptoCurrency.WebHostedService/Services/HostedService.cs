using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

public class HostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random;
    private readonly IHubContext<PriceHistoryHub> _hubContext;

    public HostedService(IServiceProvider serviceProvider, IHubContext<PriceHistoryHub> hubContext)
    {
        _serviceProvider = serviceProvider;
        _random = new Random();
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var priceHistoryService = scope.ServiceProvider.GetRequiredService<IPriceHistoryService>();
                try
                {
                    var result = await priceHistoryService.GeneratePriceHistory();
                    Console.WriteLine($"Generated price for {result.Name} at {DateTime.UtcNow}");

                    await _hubContext.Clients.All.SendAsync("PriceUpdate", result);

                    // random delay between 300ms and 1500ms
                    int delay = _random.Next(300, 1500);
                    await Task.Delay(delay, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating price: {ex.Message}");
                }
            }
        }
    }
}
