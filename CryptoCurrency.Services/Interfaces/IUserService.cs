using CryptoCurrency.Model.DTO;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserData();
    }
}
