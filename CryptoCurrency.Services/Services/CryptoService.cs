using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryptoCurrency.Services.Services
{
    public class CryptoService : BaseService, ICryptoService
    {
        public CryptoService(AppDbContext dbContext, IUserContextService userContextService, UserManager<AppUser> userManager, IMapper mapper) : base(dbContext, userManager, mapper, userContextService) { }

        public async Task<List<CryptoDTO>> GetAllAsync()
        {
            var cryptoList = await _dbContext.Crypto.Include(c => c.PriceHistory).ToListAsync();
            return cryptoList == null ? new List<CryptoDTO>() : _mapper.Map<List<CryptoDTO>>(cryptoList);
        }
    }
}
