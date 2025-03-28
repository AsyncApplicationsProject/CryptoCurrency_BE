using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class TradeService : BaseService, ITradeService
{
    protected readonly IJwtService _jwtService;

    public TradeService(AppDbContext dbContext, UserManager<AppUser> userManager, IMapper mapper, IUserContextService userContextService, IJwtService jwtService)
        : base(dbContext, userManager, mapper, userContextService)
    {
        _jwtService = jwtService;
    }

    public async Task<TradeResult> Buy(string symbol, int amount, string userId)
    {
        if (userId == null)
            return TradeResult.Failure("Client is not authenticated.");
        if (amount <= 0)
            return TradeResult.Failure("Invalid amount.");

        var crypto = await _dbContext.Crypto.Include(c => c.PriceHistory).FirstOrDefaultAsync(c => c.Symbol == symbol);
        if (crypto == null)
            return TradeResult.Failure("Invalid crypto symbol.");

        var user = await _dbContext.AppUsers.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return TradeResult.Failure("User not found.");

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
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
                    CryptoSymbol = symbol,
                    Crypto = crypto,
                    Amount = amount
                });
            }

            var lastPrice = crypto.PriceHistory.LastOrDefault()?.Price ?? 0;
            if (lastPrice == 0)
                return TradeResult.Failure("Price data not available for the selected crypto.");
            user.Balance -= amount * lastPrice;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return TradeResult.Success(user.Balance, crypto.Symbol, user.Wallet.FirstOrDefault(w => w.CryptoSymbol == symbol)!.Amount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return TradeResult.Failure($"Error while saving changes: {ex.Message}");
        }
    }

    public async Task<TradeResult> Sell(string symbol, int amount, string userId)
    {
        if (userId == null)
            return TradeResult.Failure("Client is not authenticated.");
        if (amount <= 0)
            return TradeResult.Failure("Invalid amount.");

        var crypto = await _dbContext.Crypto.Include(c => c.PriceHistory).FirstOrDefaultAsync(c => c.Symbol == symbol);
        if (crypto == null)
            return TradeResult.Failure("Invalid crypto symbol.");

        var user = await _dbContext.AppUsers.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return TradeResult.Failure("User not found.");

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var walletItem = user.Wallet.FirstOrDefault(w => w.CryptoSymbol == symbol);
            if (walletItem == null || walletItem.Amount < amount)
                return TradeResult.Failure("Insufficient funds.");

            var lastPrice = crypto.PriceHistory.LastOrDefault()?.Price ?? 0;
            if (lastPrice == 0)
                return TradeResult.Failure("Price data not available for the selected crypto.");
            user.Balance += amount * lastPrice;

            walletItem.Amount -= amount;

            if (walletItem.Amount <= 0)
                _dbContext.UserCryptos.Remove(walletItem);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return TradeResult.Success(user.Balance, crypto.Symbol, walletItem.Amount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return TradeResult.Failure($"Error while saving changes: {ex.Message}");
        }
    }
}
