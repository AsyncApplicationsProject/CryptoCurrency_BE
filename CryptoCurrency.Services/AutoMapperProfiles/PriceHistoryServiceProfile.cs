using AutoMapper;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.AutoMapperProfiles
{
    public class PriceHistoryServiceProfile : Profile
    {
        public PriceHistoryServiceProfile()
        {
            CreateMap<PriceHistory, PriceHistoryDTO>();
        }
    }
}
