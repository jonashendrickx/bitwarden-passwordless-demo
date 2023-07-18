namespace Passwordless.YourBackend.Passwordless;

public class PasswordlessOptions
{
    public const string Root = "Passwordless";
    public string BaseUrl { get; set; }
    public string Secret { get; set; }
}