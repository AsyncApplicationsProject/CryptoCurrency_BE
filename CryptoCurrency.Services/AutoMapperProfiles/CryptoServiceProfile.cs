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
        }
    }
}
