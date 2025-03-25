using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoCurrency.Services.Services
{
    public class PriceHistoryService : IPriceHistoryService
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IMapper _mapper;
        private Random _random;

        public PriceHistoryService(AppDbContext dbContext, IMapper mapper) 
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _random = new Random();
        }

        public async Task<CryptoDTO> GeneratePriceHistory()
        {
            var cryptos = await _dbContext.Crypto.ToListAsync();
            var item = _random.Next(cryptos.Count);

            PriceHistoryDTO newPriceHistoryDTO = new PriceHistoryDTO();
            newPriceHistoryDTO.Date = DateTime.UtcNow;
            newPriceHistoryDTO.Price = _random.Next(5000, 60000);

            cryptos[item].PriceHistory.Clear();

            CryptoDTO cryptoDTO = _mapper.Map<CryptoDTO>(cryptos[item]);
            cryptoDTO.PriceHistory.Add(newPriceHistoryDTO);

            return cryptoDTO;
        }
    }
}
