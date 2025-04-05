using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.Model.DTO;
using Microsoft.AspNetCore.SignalR;
using CryptoCurrency.WebAPI.Hubs;

namespace CryptoCurrency.Services.Services
{
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
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var priceHistoryService = scope.ServiceProvider.GetRequiredService<IPriceHistoryService>();

                        var result = await priceHistoryService.GeneratePriceHistory();
                        Console.WriteLine($"Generated price for {result.Name} at {DateTime.UtcNow}");

                        await NotifyClientsAsync(result);

                        // random delay between 300ms and 1500ms
                        int delay = _random.Next(300, 1500);
                        await Task.Delay(delay, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating price: {ex.Message}");
                }
            }
        }

        private async Task NotifyClientsAsync(CryptoPriceDTO dto)
        {
            await _hubContext.Clients.All.SendAsync("PriceUpdate", dto);
        }
    }
}
