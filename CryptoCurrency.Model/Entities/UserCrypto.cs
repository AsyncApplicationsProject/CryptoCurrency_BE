using System.ComponentModel.DataAnnotations;

namespace CryptoCurrency.Model.Entities
{
    public class UserCrypto
    {
        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public string CryptoSymbol { get; set; } = null!;
        public Crypto Crypto { get; set; } = null!;
        public int Amount { get; set; }
    }
}
