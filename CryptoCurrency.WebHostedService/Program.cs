using CryptoCurrency.DAL.EF;
using CryptoCurrency.Services.AutoMapperProfiles;
using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.Services.Services;
using CryptoCurrency.WebAPI.Hubs;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoCurrencyDb")));

        // Dodanie SignalR
        builder.Services.AddSignalR();

        builder.Services.AddScoped<IPriceHistoryService, PriceHistoryService>();
        builder.Services.AddHostedService<HostedService>();

        builder.Services.AddAutoMapper(typeof(PriceHistoryServiceProfile));

        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("http://localhost:4200")  // Ustawienie frontendowego adresu
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();  // Zezwala na dane uwierzytelniaj¹ce (ciasteczka, tokeny)
            });
        });

        var app = builder.Build();

        // U¿ywanie nowej polityki CORS
        app.UseCors("AllowSpecificOrigin");

        app.MapHub<PriceHistoryHub>("/priceHistoryHub");

        app.Run();
    }
}