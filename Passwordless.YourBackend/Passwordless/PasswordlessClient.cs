using Microsoft.AspNetCore.Mvc;
using Passwordless.YourBackend.Passwordless.Models;

namespace Passwordless.YourBackend.Passwordless;

public class PasswordlessClient : IPasswordlessClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public PasswordlessClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<TokenResponse> RegisterTokenAsync(string userId, string username, string alias)
    { 
        var payload = new
        {
            userId,
            username,
            Aliases = new[] { alias }
        };

        var client = _httpClientFactory.CreateClient("Passwordless");
        var request = await client.PostAsJsonAsync("register/token", payload);

        if (request.IsSuccessStatusCode)
        {
            var token = await request.Content.ReadFromJsonAsync<TokenResponse>();
            return token;
        }

        var error = await request.Content.ReadFromJsonAsync<ProblemDetails>();
        throw new PasswordlessException(error);
    }

    public async Task<SignInResponse> SignInAsync(string signInToken)
    {
        var payload = new
        {
            token = signInToken
        };
        
        var client = _httpClientFactory.CreateClient("Passwordless");
        var request = await client.PostAsJsonAsync("signin/verify", payload);

        if (request.IsSuccessStatusCode)
        {
            var verifiedToken = await request.Content.ReadFromJsonAsync<SignInResponse>();
            return verifiedToken;
        }

        var error = await request.Content.ReadFromJsonAsync<ProblemDetails>();
        throw new PasswordlessException(error);
    }
}