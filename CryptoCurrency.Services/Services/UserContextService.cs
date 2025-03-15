using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CryptoCurrency.Services.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public UserContextService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        //public async Task<AppUser?> GetAppUser(string? email, string? userName)
        //{
        //    if (!string.IsNullOrWhiteSpace(email))
        //    {
        //        return await _userManager.FindByEmailAsync(email);
        //    }
        //    else if (!string.IsNullOrWhiteSpace(userName))
        //    {
        //        return await _userManager.FindByNameAsync(userName);
        //    }
        //    return null;
        //}

        //public async Task<AppUser> GetAppUser()
        //{
        //    var userId = GetUserId();
        //    if (userId == null)
        //        throw new UnauthorizedAccessException("The user is not authenticated.");

        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        throw new UnauthorizedAccessException("The authenticated user could not be found.");
        //    return user;
        //}
    }
}
