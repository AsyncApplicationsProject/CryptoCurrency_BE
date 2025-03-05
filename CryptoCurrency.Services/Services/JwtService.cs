using CryptoCurrency.Model.Entities;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CryptoCurrency.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration Configuration;
        private readonly UserManager<AppUser> UserManager;

        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            Configuration = configuration;
            UserManager = userManager;
        }

        public JwtSecurityToken GenerateJwt(AppUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName))
                throw new Exception("User is not valid");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured")));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddSeconds(
                    double.Parse(Configuration["JWTExtraSettings:TokenExpirySeconds"] ?? "4500000")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public string WriteToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured"));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ClockSkew = TimeSpan.FromSeconds(5),
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
