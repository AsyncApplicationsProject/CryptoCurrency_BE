using AutoMapper;
using CryptoCurrency.DAL.Seed;
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
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });

            AppUser newUser = Mapper.Map<AppUser>(model);
            newUser.UserName = model.Email.Split('@')[0];

            var result = await UserManager.CreateAsync(newUser, model.Password);
            return result;
        }
    }
}
