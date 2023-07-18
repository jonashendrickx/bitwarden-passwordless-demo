using Passwordless.YourBackend.Passwordless.Models;

namespace Passwordless.YourBackend.Passwordless;

public interface IPasswordlessClient
{
    Task<TokenResponse> RegisterTokenAsync(string userId, string username, string alias);
    Task<SignInResponse> SignInAsync(string token);
}