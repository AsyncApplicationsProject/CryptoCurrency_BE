using AutoMapper;
using CryptoCurrency.Model.DTO.Auth;
using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CryptoCurrency.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService JwtService;
        private readonly UserManager<AppUser> UserManager;
        private readonly IUserContextService UserContextService;
        private readonly IMapper Mapper;

        public UserService(IJwtService jwtService, UserManager<AppUser> userManager, IUserContextService userContextService, IMapper mapper)
        {
            JwtService = jwtService;
            UserManager = userManager;
            UserContextService = userContextService;
            Mapper = mapper;
        }

        public async Task<string> LoginUserAsync(LoginModel model)
        {
            AppUser? user = null;

            // Find user by email
            if (!string.IsNullOrEmpty(model.Email))
                user = await UserManager.FindByEmailAsync(model.Email);

            // If user is not found or password is incorrect, throw an exception
            if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("403");

            var token = JwtService.GenerateJwt(user);

            return JwtService.WriteToken(token);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegistrationModel model)
        {
            var existingUser = await UserManager.FindByNameAsync(model.Email);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });

            var newUser = new AppUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await UserManager.CreateAsync(newUser, model.Password);
            return result;
        }
    }
}
