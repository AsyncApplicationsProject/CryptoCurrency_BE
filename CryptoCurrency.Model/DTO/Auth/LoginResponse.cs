namespace CryptoCurrency.Model.DTO.Auth
{
    public class LoginResponse
    {
        public string JwtToken { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
    }
}
