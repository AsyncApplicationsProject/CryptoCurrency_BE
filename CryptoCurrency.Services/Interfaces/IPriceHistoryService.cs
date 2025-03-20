using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IPriceHistoryService
    {
        Task<CryptoDTO> GeneratePriceHistory();
    }
}
