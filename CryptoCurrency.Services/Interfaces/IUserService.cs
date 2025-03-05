using CryptoCurrency.Model.DTO.Auth;
using Microsoft.AspNetCore.Identity;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegistrationModel model);
        Task<string> LoginUserAsync(LoginModel model);
    }
}
