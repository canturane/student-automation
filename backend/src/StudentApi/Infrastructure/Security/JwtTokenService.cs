using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Domain;

namespace StudentApi.Infrastructure.Security;

public interface IJwtTokenService
{
    string CreateToken(int userId, string email, UserRole role);
}

public class JwtTokenService(IConfiguration cfg) : IJwtTokenService
{
    public string CreateToken(int userId, string email, UserRole role)
    {
        
        var issuer = cfg["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer missing in configuration");
        var audience = cfg["Jwt:Audience"] ?? throw new Exception("Jwt:Audience missing in configuration");
        var keyValue = cfg["Jwt:Key"] ?? throw new Exception("Jwt:Key missing in configuration");

        if (string.IsNullOrWhiteSpace(keyValue))
            throw new Exception("Jwt:Key is empty!");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

        // --- Claims ---
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            // Artık role PascalCase geliyor: Admin, Teacher, Student
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
