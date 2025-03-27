using AutoMapper;
using CryptoCurrency.DAL.EF;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CryptoCurrency.Services.Services
{
    public abstract class BaseService
    {
        protected readonly AppDbContext _dbContext;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IMapper _mapper;
        protected readonly IUserContextService _userContextService;

        public BaseService(AppDbContext dbContext, UserManager<AppUser> userManager, IMapper mapper, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _userContextService = userContextService;
        }
    }
}
