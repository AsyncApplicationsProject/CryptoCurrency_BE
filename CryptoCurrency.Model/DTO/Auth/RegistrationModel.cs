using System.ComponentModel.DataAnnotations;

namespace CryptoCurrency.Model.DTO.Auth
{
    public class RegistrationModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
