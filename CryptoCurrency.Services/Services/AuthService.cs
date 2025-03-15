using AutoMapper;
using CryptoCurrency.Model.DTO.Auth;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CryptoCurrency.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService JwtService;
        private readonly UserManager<AppUser> UserManager;
        private readonly IMapper Mapper;

        public AuthService(IJwtService jwtService, UserManager<AppUser> userManager, IMapper mapper)
        {
            JwtService = jwtService;
            UserManager = userManager;
            Mapper = mapper;
        }

        public async Task<string> LoginUserAsync(LoginModel model)
        {
            AppUser? user = null;

            // Find user by email
            if (!string.IsNullOrEmpty(model.Email))
                user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email");
            if (!await UserManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("Invalid password");

            var token = JwtService.GenerateJwt(user);

            return JwtService.WriteToken(token);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegistrationModel model)
        {
            var existingUser = await UserManager.FindByNameAsync(model.Email);
            if (existingUser != null)
                return IdentityResult.Failed();

            AppUser newUser = Mapper.Map<AppUser>(model);

            var result = await UserManager.CreateAsync(newUser, model.Password);
            return result;
        }
    }
}
