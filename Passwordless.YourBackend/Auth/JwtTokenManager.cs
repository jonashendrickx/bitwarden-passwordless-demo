using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Passwordless.YourBackend.Database.Models;

namespace Passwordless.YourBackend.Auth;

public sealed class JwtTokenManager : IJwtTokenManager
{
    private readonly IOptionsMonitor<JwtOptions> _options;
    
    public JwtTokenManager(IOptionsMonitor<JwtOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    
    public string GenerateToken(User user)
    {
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.CurrentValue.Secret));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var identityClaims = user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
        identityClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(identityClaims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _options.CurrentValue.ValidIssuer,
            Audience = _options.CurrentValue.ValidAudience,
            SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        tokenDescriptor.Claims = new Dictionary<string, object>();
        

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}