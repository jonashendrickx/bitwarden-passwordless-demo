namespace Passwordless.YourBackend.Contracts.Users;

public class SignUpRequest
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? DeviceName { get; set; }
}