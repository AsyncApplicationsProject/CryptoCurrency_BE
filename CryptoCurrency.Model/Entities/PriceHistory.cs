using System.ComponentModel.DataAnnotations;

namespace CryptoCurrency.Model.Entities
{
    public class PriceHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string CryptoSymbol { get; set; } = null!;
        public Crypto Crypto { get; set; } = null!;
    }
}
