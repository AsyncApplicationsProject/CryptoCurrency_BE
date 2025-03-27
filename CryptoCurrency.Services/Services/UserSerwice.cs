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
    public class UserSerwice : BaseService, IUserService
    {
        public UserSerwice(AppDbContext dbContext, UserManager<AppUser> userManager, IMapper mapper, IUserContextService userContextService) : base(dbContext, userManager, mapper, userContextService) { }

        public async Task<UserDTO> GetUserData()
        {
            var userId = _userContextService.GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("The user is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("The authenticated user could not be found.");

            var userCryptos = await _dbContext.UserCryptos
                .Where(uc => uc.AppUserId == user.Id)
                .ToListAsync();

            var userDTO = _mapper.Map<UserDTO>(user);

            userDTO.Wallet = _mapper.Map<List<UserCryptoDTO>>(userCryptos);

            return userDTO;
        }
    }
}
