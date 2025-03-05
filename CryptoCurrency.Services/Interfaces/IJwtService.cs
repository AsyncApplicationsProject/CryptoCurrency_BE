using CryptoCurrency.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IJwtService
    {
        public JwtSecurityToken GenerateJwt(AppUser user);
        public string WriteToken(JwtSecurityToken token);
        public ClaimsPrincipal? ValidateJwt(string token);
    }
}
