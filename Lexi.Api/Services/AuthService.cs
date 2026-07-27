using Google.Apis.Auth;
using Lexi.Api.DTOs.Auth;
using Lexi.Domain.Entities;
using Lexi.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Lexi.Api.Services;

public class AuthService
{
    private readonly LexiDbContext _db;
    private readonly JwtTokenService _jwtService;
    private readonly IConfiguration _config;

    public AuthService(
        LexiDbContext db,
        JwtTokenService jwtService,
        IConfiguration config
    )
    {
        _db = db;
        _jwtService = jwtService;
        _config = config;
    }

    public async Task<(AuthResponse? result, string? error)> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null) return (null, "Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.Fullname,
            LoginType = "email",
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);

        _db.UserSettings.Add(new UserSettings
        {
            UserId = user.Id,
            NotificationEnabled = true,
            UpdatedAt = DateTime.UtcNow,
        });

        await _db.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);
        return (ToAuthResponse(user, token), null);
    }
    public async Task<(AuthResponse? result, string? error)> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || user.LoginType != "email" || user.PasswordHash == null)
        {
            return (null, "User not found or the user was registered with another method");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return (null, "Invalid password");
        }

        var token = _jwtService.GenerateToken(user);
        return (ToAuthResponse(user, token), null);
    }

    public async Task<(AuthResponse? result, string? error)> GoogleLoginAsync(GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _config["Google:ClientId"] }
            };
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        }
        catch
        {
            return (null, "Invalid google token");
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = payload.Email,
                FullName = payload.Name,
                AvatarUrl = payload.Picture,
                LoginType = "google",
                EmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _db.Users.Add(user);
            _db.UserSettings.Add(new UserSettings
            {
                UserId = user.Id,
                NotificationEnabled = true,
                UpdatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
        var token = _jwtService.GenerateToken(user);
        return (ToAuthResponse(user, token), null);
    }

    private static AuthResponse ToAuthResponse(User user, string token) => new()
    {
        AccessToken = token,
        UserId = user.Id,
        Email = user.Email,
        FullName = user.FullName,
        AvatarUrl = user.AvatarUrl,
        LoginType = user.LoginType,
    };
}