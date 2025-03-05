using System.ComponentModel.DataAnnotations;

namespace CryptoCurrency.Model.DTO.Auth
{
    public class RegistrationModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
