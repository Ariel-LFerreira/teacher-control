using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeacherControl.Models;

namespace TeacherControl.Services;

public class TokenService(IConfiguration config)
{
    public string GenerationToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"])
        );
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())   // ex: "Admin"
        };

        var token = new JwtSecurityToken(
            issuer:    config["Jwt:Issuer"],
            audience:  config["Jwt:Audience"],
            claims:    claims,
            expires:   DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    
    }
    
}