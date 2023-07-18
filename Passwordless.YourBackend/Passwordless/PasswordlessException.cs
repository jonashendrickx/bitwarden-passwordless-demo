using Microsoft.AspNetCore.Mvc;

namespace Passwordless.YourBackend.Passwordless;

public class PasswordlessException : Exception
{
    public PasswordlessException(ProblemDetails error) : base(error?.Title)
    {
    }
}