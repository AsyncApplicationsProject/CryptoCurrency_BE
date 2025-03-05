using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptoCurrency.Services.Services
{
    public class CryptoService : BaseService, ICryptoService
    {
        public CryptoService(AppDbContext dbContext, IUserContextService userContextService, UserManager<AppUser> userManager, IMapper mapper) : base(dbContext, userContextService, userManager, mapper) { }

        public async Task<List<CryptoDTO>> GetAllAsync()
        {
            var cryptoList = await DbContext.Crypto.Include(c => c.PriceHistory).ToListAsync();
            return cryptoList == null ? new List<CryptoDTO>() : Mapper.Map<List<CryptoDTO>>(cryptoList);
        }
    }
}
