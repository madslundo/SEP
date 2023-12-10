using Domain;
using EFC;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Services.Impls;

public class InMemoryUserService : IUserService
{
    private readonly SetupContext context;

    public InMemoryUserService(SetupContext context)
    {
        this.context = context;
    }

    public async Task<User?> GetUserAsync(string username)
    {
        // Query the SQLite database for the user with the given username
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName.Equals(username));

        return user;
    }
}