using AutoMapper;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.AutoMapperProfiles
{
    public class CryptoServiceProfile : Profile
    {
        public CryptoServiceProfile()
        {
            CreateMap<Crypto, CryptoDTO>();
            CreateMap<Crypto, CryptoPriceDTO>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.PriceHistory.Last().Date))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceHistory.Last().Price));
        }
    }
}
