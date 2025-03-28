using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Model.Entities
{
    public class PriceHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string CryptoSymbol { get; set; } = null!;
        public Crypto Crypto { get; set; } = null!;
    }
}
