using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeacherControl.Models;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;
    
    public string GenerateToken(User user)
    {
        if (user.Role == null)
            throw new Exception("User role not loaded");
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "")
            //new Claim(ClaimTypes.Role, user.Role!.Name!.ToString())   // ex: "Admin"
        };
        
        //SE ESTIVER CONFIGURADO O TEMPO
        var timeExpires = _config["Jwt:ExpireMinutes"];
        if (timeExpires == null)
            timeExpires = "60";

        var token = new JwtSecurityToken(
            issuer:    _config["Jwt:Issuer"],
            audience:  _config["Jwt:Audience"],
            claims:    claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(timeExpires)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}