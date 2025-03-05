using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoCurrency.DAL.Seed
{
    public static class Seed
    {
        private static readonly Random random = new Random();

        public static async Task Init(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            await ClearDatabaseAsync(dbContext);

            // Generujemy kryptowaluty z historią cen i dodajemy do bazy danych
            var cryptos = GenerateCryptosWithPriceHistory();
            await AddEntitiesAsync(dbContext, cryptos);

            // Pobieramy dodane kryptowaluty z bazy
            var existingCryptos = await dbContext.Set<Crypto>().ToListAsync();

            // Generujemy użytkowników z portfelami na podstawie istniejących kryptowalut
            var users = GetUsers(existingCryptos);
            await CreateUserWithPasswordAsync(userManager, users);

            Console.WriteLine("Inicjalizacja zakończona.");
        }

        private static async Task ClearDatabaseAsync(AppDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.Set<AppUser>());
            dbContext.RemoveRange(dbContext.Set<Crypto>());
            dbContext.RemoveRange(dbContext.Set<PriceHistory>());
            dbContext.RemoveRange(dbContext.Set<UserCrypto>());

            await dbContext.SaveChangesAsync();
            Console.WriteLine("Wszystkie dane zostały usunięte z bazy danych.");
        }

        private static async Task AddEntitiesAsync<T>(AppDbContext dbContext, IEnumerable<T> entities) where T : class
        {
            await dbContext.Set<T>().AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }

        private static string GeneratePhoneNumber()
        {
            string phoneNumber = string.Empty;
            for (int i = 0; i < 9; i++)
                phoneNumber += random.Next(0, 10);

            return phoneNumber;
        }

        private static async Task CreateUserWithPasswordAsync(UserManager<AppUser> userManager, List<AppUser> users)
        {
            foreach (var user in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email ?? "");
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(user, "Haslo123!");
                }
            }
        }

        private static List<AppUser> GetUsers(List<Crypto> existingCryptos)
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "user1",
                    FirstName = "User1",
                    LastName = "User1",
                    Email = "user1@example.com",
                    PhoneNumber = GeneratePhoneNumber(),
                    Balance = GenerateBalance(),
                    Wallet = GenerateWallet(existingCryptos)
                },
                new AppUser
                {
                    UserName = "user2",
                    FirstName = "User2",
                    LastName = "User2",
                    Email = "user2@example.com",
                    PhoneNumber = GeneratePhoneNumber(),
                    Balance = GenerateBalance(),
                    Wallet = GenerateWallet(existingCryptos)
                },
                new AppUser
                {
                    UserName = "user3",
                    FirstName = "User3",
                    LastName = "User3",
                    Email = "user3@example.com",
                    PhoneNumber = GeneratePhoneNumber(),
                    Balance = GenerateBalance(),
                    Wallet = GenerateWallet(existingCryptos)
                },
            };

            return users;
        }

        private static List<Crypto> GenerateCryptosWithPriceHistory()
        {
            var cryptos = new List<Crypto>
            {
                new Crypto { Symbol = "BTC", Name = "Bitcoin" },
                new Crypto { Symbol = "ETH", Name = "Ethereum" },
                new Crypto { Symbol = "LTC", Name = "Litecoin" }
            };

            foreach (var crypto in cryptos)
            {
                crypto.PriceHistory = GeneratePriceHistory(crypto.Symbol, random.Next(5000, 60000));
            }

            return cryptos;
        }

        private static List<PriceHistory> GeneratePriceHistory(string cryptoSymbol, double startingPrice)
        {
            var priceHistory = new List<PriceHistory>();
            double currentPrice = startingPrice;

            for (int i = 100; i >= 0; i--)
            {
                double priceChange = currentPrice * (random.NextDouble() * 0.1 - 0.05);
                currentPrice = Math.Round(currentPrice + priceChange, 2);

                priceHistory.Add(new PriceHistory
                {
                    Date = DateTime.Now.AddHours(-i),
                    Price = currentPrice,
                    CryptoSymbol = cryptoSymbol
                });
            }

            return priceHistory;
        }

        private static List<UserCrypto> GenerateWallet(List<Crypto> existingCryptos)
        {
            var wallet = new List<UserCrypto>();

            foreach (var crypto in existingCryptos)
            {
                if (random.NextDouble() > 0.5) // Losowa decyzja, czy dodać daną kryptowalutę
                {
                    var userCrypto = new UserCrypto
                    {
                        CryptoSymbol = crypto.Symbol,
                        Amount = random.Next(1, 11)
                    };
                    wallet.Add(userCrypto);
                }
            }

            return wallet;
        }

        private static decimal GenerateBalance()
        {
            return Math.Round((decimal)(random.NextDouble() * 10000), 2);
        }
    }
}