using Passwordless.YourBackend.Database.Models;

namespace Passwordless.YourBackend.Auth;

public interface IJwtTokenManager
{
    string GenerateToken(User user);
}