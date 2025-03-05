using CryptoCurrency.Model.Entities;

namespace CryptoCurrency.Services.Interfaces;

public interface IUserContextService
{
    Task<AppUser?> GetAppUser(string? email, string? userName);
    Task<AppUser> GetAppUser();
    string? GetUserName();
}
