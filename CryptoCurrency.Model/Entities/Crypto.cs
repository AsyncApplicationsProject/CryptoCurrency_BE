using System.ComponentModel.DataAnnotations;

namespace CryptoCurrency.Model.Entities
{
    public class Crypto
    {
        [Key]
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<PriceHistory> PriceHistory { get; set; } = new();
    }
}
