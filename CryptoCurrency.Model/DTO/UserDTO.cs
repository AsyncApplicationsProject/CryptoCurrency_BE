namespace CryptoCurrency.Model.DTO
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<UserCryptoDTO> Wallet { get; set; } = new List<UserCryptoDTO>();
    }
}
