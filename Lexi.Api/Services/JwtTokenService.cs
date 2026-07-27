using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lexi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Lexi.Api.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        // Secret key dùng để ký và xác thực JWT token. -> Phải được giữ bí mật, không được đưa lên frontend.
        var secret = jwtSettings["Secret"];

        // Tên hệ thống phát hành token. -> Dùng để xác định token này được tạo bởi service nào.
        var issuer = jwtSettings["Issuer"];

        // Đối tượng được phép sử dụng token. -> Ví dụ: Web app, Mobile app hoặc một client cụ thể.
        var audience = jwtSettings["Audience"];

        // Thời gian token có hiệu lực (tính bằng phút) -> Nếu không cấu hình thì mặc định là 10080 phút = 7 ngày.
        var expireMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "10080");
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(expireMinutes), signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}