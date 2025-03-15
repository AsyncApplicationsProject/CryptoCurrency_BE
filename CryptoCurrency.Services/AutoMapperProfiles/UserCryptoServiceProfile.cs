using AutoMapper;
using CryptoCurrency.Model.DTO;
using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.AutoMapperProfiles
{
    public class UserCryptoServiceProfile : Profile
    {
        public UserCryptoServiceProfile() 
        {
            CreateMap<UserCrypto, UserCryptoDTO>();
        }
    }
}
