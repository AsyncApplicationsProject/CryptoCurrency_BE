using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Model.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
        public List<UserCrypto> Wallet { get; set; } = new List<UserCrypto>();
    }
}
