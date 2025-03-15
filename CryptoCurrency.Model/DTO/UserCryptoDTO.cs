namespace CryptoCurrency.Model.DTO
{
    public class UserCryptoDTO
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public string CryptoSymbol { get; set; } = null!;
        public int Amount { get; set; }
    }
}
