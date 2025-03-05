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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();

            // Register AutoMapper profiles
            services.AddAutoMapper(
                typeof(CryptoServiceProfile),
                typeof(PriceHistoryServiceProfile)
            );
        }
    }
}
