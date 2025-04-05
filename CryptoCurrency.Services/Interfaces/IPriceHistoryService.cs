using CryptoCurrency.Model.DTO;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IPriceHistoryService
    {
        Task<CryptoPriceDTO> GeneratePriceHistory();
    }
}
