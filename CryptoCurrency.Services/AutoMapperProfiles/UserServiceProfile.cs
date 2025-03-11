using AutoMapper;
using CryptoCurrency.DAL.Seed;
using CryptoCurrency.Model.DTO.Auth;
using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.AutoMapperProfiles
{
    public class UserServiceProfile : Profile
    {
        public UserServiceProfile() 
        {
            CreateMap<RegistrationModel, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => Seed.GenerateBalance()))
                .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => new List<UserCrypto>()))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
