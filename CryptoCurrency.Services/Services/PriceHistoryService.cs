using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoCurrency.Services.Services
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private Random _random;

        public PriceHistoryService(AppDbContext dbContext, IMapper mapper) 
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _random = new Random();
        }

        public async Task<CryptoPriceDTO> GeneratePriceHistory()
        {
            var cryptos = await _dbContext.Crypto.Include(c => c.PriceHistory).ToListAsync();
            var item = _random.Next(cryptos.Count);

            PriceHistory newPriceHistory = new PriceHistory();
            newPriceHistory.Date = DateTime.Now;
            newPriceHistory.Price = _random.Next(20000, 60000);
            newPriceHistory.CryptoSymbol = cryptos[item].Symbol;
            newPriceHistory.Crypto = cryptos[item];

            cryptos[item].PriceHistory.Add(newPriceHistory);

            await _dbContext.SaveChangesAsync();

            CryptoPriceDTO DTO = _mapper.Map<CryptoPriceDTO>(cryptos[item]);

            return DTO;
        }
    }
}
