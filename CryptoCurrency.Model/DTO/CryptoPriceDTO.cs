using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Model.DTO
{
    public class CryptoPriceDTO
    {
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
