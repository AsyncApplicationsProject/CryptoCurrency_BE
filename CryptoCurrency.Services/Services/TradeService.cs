using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoCurrency.Services.Services
{
    public class TradeService : ITradeService
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IJwtService _jwtService;

        public TradeService(AppDbContext dbContext, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<bool> Buy(string symbol, int amount, string token)
        {
            var userId = _jwtService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(token) || userId == null)
                return false;

            if (amount <= 0)
                return false;

            var crypto = _dbContext.Crypto.FirstOrDefault(c => c.Symbol == symbol);
            if (crypto == null)
                return false;

            var user = await _dbContext.AppUsers.Include(u => u.Wallet)
                                              .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;


            var walletItem = user.Wallet.FirstOrDefault(w => w.CryptoSymbol == symbol);
            if (walletItem != null)
            {
                walletItem.Amount += amount;
            }
            else
            {
                user.Wallet.Add(new UserCrypto
                {
                    AppUserId = userId,
                    AppUser = user,
                    CryptoSymbol = symbol,
                    Crypto = crypto,
                    Amount = amount
                });
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Task<bool> Sell(string symbol, int amount, string token)
        {
            throw new NotImplementedException();
        }
    }
}
