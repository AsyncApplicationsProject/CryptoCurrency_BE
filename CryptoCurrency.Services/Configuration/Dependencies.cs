using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.Services.AutoMapperProfiles;
using CryptoCurrency.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoCurrency.Services.Configuration
{
    public class Dependencies
    {
        public static void Register(IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IUserService, UserSerwice>();
            services.AddScoped<IPriceHistoryService, PriceHistoryService>();
            services.AddScoped<ITradeService, TradeService>();

            // Register AutoMapper profiles
            services.AddAutoMapper(
                typeof(CryptoServiceProfile),
                typeof(PriceHistoryServiceProfile),
                typeof(TradeService),
                typeof(UserCryptoServiceProfile),
                typeof(UserServiceProfile)
            );
        }
    }
}
