namespace CryptoCurrency.Model.DTO
{
    public class CryptoDTO
    {
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<PriceHistoryDTO> PriceHistory { get; set; } = new();
    }
}
