
using Lexi.Api.DTOs.Auth;
using Lexi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lexi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var (result, error) = await _authService.RegisterAsync(request);
        if (error != null)
        {
            return BadRequest(new { Error = error });
        }
        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var (result, error) = await _authService.LoginAsync(request);
        if (error != null) return Unauthorized(new { message = error });
        return Ok(result);
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
    {
        var (result, error) = await _authService.GoogleLoginAsync(request);
        if (error != null) return Unauthorized(new { message = error });
        return Ok(result);
    }
}