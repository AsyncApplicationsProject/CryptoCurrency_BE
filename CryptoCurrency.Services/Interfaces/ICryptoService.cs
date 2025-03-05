using CryptoCurrency.Model.DTO;

namespace CryptoCurrency.Services.Interfaces
{
    public interface ICryptoService
    {
        Task<List<CryptoDTO>> GetAllAsync();
    }
}
