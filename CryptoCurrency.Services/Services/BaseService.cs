using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CryptoCurrency.Services.Services
{
    public abstract class BaseService
    {
        protected readonly AppDbContext DbContext;
        protected readonly IUserContextService UserContextService;
        protected readonly UserManager<AppUser> UserManager;
        protected readonly IMapper Mapper;

        public BaseService(AppDbContext dbContext, IUserContextService userContextService, UserManager<AppUser> userManager, IMapper mapper)
        {
            DbContext = dbContext;
            UserContextService = userContextService;
            UserManager = userManager;
            Mapper = mapper;
        }
    }
}
