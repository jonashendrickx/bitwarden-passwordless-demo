namespace Passwordless.YourBackend.Auth;

public sealed class JwtOptions
{
    public const string Root = "JWT";
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
}