using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Passwordless.YourBackend.Auth;
using Passwordless.YourBackend.Contracts.Users;
using Passwordless.YourBackend.Database;
using Passwordless.YourBackend.Database.Models;
using Passwordless.YourBackend.Passwordless;

namespace Passwordless.YourBackend.Controllers;

[ApiController]
public class UserController : Controller
{
    private readonly IPasswordlessClient _passwordlessClient;
    private readonly IJwtTokenManager _tokenManager;

    public UserController(
        IPasswordlessClient passwordlessClient,
        IJwtTokenManager tokenManager)
    {
        _passwordlessClient = passwordlessClient;
        _tokenManager = tokenManager;
    }

    /// <summary>
    /// Creates a new user with its first token.
    /// </summary>
    /// <returns></returns>
    [HttpPost("/signup")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUpAsync(SignUpRequest request)
    {
        await using var dbContext = new YourBackendContext();
        var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var newUser = new User();
            newUser.Username = request.Username;
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();
            if (newUser.Id == default)
            {
                var problem = new ProblemDetails
                {
                    Status = 400,
                    Title = "Failed to create user."
                };
                return BadRequest(problem);
            }
            var token = await _passwordlessClient.RegisterTokenAsync(newUser.Id.ToString(), request.Username, request.DeviceName);
            await transaction.CommitAsync();
            return Ok(token);
        }
        catch (PasswordlessException e)
        {
            await transaction.RollbackAsync();
            var problem = new ProblemDetails
            {
                Status = 400,
                Title = e.Message
            };
            return BadRequest(problem);
        }
    }

    [HttpGet]
    [Route("/signin")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInAsync(string token)
    {
        try
        {
            var result = await _passwordlessClient.SignInAsync(token);

            await using var dbContext = new YourBackendContext();
            var userId = Guid.Parse(result.UserId);
            var user = await dbContext.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var jwtToken = _tokenManager.GenerateToken(user);
            return Ok(new
            {
                jwt = jwtToken,
                webAuthn = result
            });
        }
        catch (PasswordlessException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/test-user")]
    [Authorize(Roles = "User")]
    public IActionResult UserTest()
    {
        return NoContent();
    }
    
    [HttpPost("/test-admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminTest()
    {
        return NoContent();
    }
}