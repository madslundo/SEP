using Domain;

namespace Blazor.Services;

public interface IUserService
{
    public Task<User> GetUserAsync(string username);
}