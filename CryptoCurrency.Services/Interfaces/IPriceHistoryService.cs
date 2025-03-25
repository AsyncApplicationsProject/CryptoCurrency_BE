using CryptoCurrency.Model.DTO;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IPriceHistoryService
    {
        Task<CryptoDTO> GeneratePriceHistory();
    }
}
