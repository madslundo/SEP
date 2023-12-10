using System.Security.Claims;
using System.Text.Json;
using Blazor.Services;
using Domain;
using Microsoft.JSInterop;

namespace Blazor.Auth;

public class AuthManagerImpl : IAuthManager
{
    private readonly IJSRuntime jsRuntime;
    private readonly IUserService userService;

    public AuthManagerImpl(IUserService userService, IJSRuntime jsRuntime)
    {
        this.userService = userService;
        this.jsRuntime = jsRuntime;
    }

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } =
        null!; // assigning to null! to suppress null warning.

    public async Task LoginAsync(string username, string password)
    {
        var user = await userService.GetUserAsync(username); // Get user from database

        ValidateLoginCredentials(password, user); // Validate input data against data from database
        // validation success
        await CacheUserAsync(user!); // Cache the user object in the browser 

        var principal = CreateClaimsPrincipal(user); // convert user object to ClaimsPrincipal

        OnAuthStateChanged?.Invoke(principal); // notify interested classes in the change of authentication state
    }

    public async Task LogoutAsync()
    {
        await ClearUserFromCacheAsync(); // remove the user object from browser cache
        var principal = CreateClaimsPrincipal(null); // create a new ClaimsPrincipal with nothing.
        OnAuthStateChanged?.Invoke(principal); // notify about change in authentication state
    }

    public async Task<ClaimsPrincipal>
        GetAuthAsync() // this method is called by the authentication framework, whenever user credentials are reguired
    {
        var user = await GetUserFromCacheAsync(); // retrieve cached user, if any

        var principal = CreateClaimsPrincipal(user); // create ClaimsPrincipal

        return principal;
    }

    private async Task<User?> GetUserFromCacheAsync()
    {
        var userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        if (string.IsNullOrEmpty(userAsJson)) return null;
        var user = JsonSerializer.Deserialize<User>(userAsJson)!;
        return user;
    }

    private static void ValidateLoginCredentials(string password, User? user)
    {
        if (user == null) throw new Exception("Username not found");

        if (!string.Equals(password, user.Password)) throw new Exception("Password incorrect");
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(User? user)
    {
        if (user != null)
        {
            var identity = ConvertUserToClaimsIdentity(user);
            return new ClaimsPrincipal(identity);
        }

        return new ClaimsPrincipal();
    }

    private async Task CacheUserAsync(User user)
    {
        var serialisedData = JsonSerializer.Serialize(user);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
    }

    private async Task ClearUserFromCacheAsync()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
    }

    private static ClaimsIdentity ConvertUserToClaimsIdentity(User user)
    {
        // here we take the information of the User object and convert to Claims
        // this is (probably) the only method, which needs modifying for your own user type
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("Name", user.Name),
            new Claim("Position", user.Position)
        };

        return new ClaimsIdentity(claims, "apiauth_type");
    }
}