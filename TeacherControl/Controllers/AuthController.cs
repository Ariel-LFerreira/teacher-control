using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService): ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var exp = User.FindFirst("exp")?.Value;

        DateTime? expiration = null;

        if (exp != null && long.TryParse(exp, out var expUnix))
        {
            expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
        }
        
        return Ok(new
        {
            Name = User.FindFirst(ClaimTypes.Name)?.Value,
            NameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = User.FindFirst(ClaimTypes.Email)?.Value,
            Role = User.FindFirst(ClaimTypes.Role)?.Value,

            Issuer = User.FindFirst("iss")?.Value,
            Audience = User.FindFirst("aud")?.Value,
            Expiration = expiration

        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var login = await authService.Login(loginRequestDto);

            if (login == null)
                return Unauthorized(new { menssage = "Invalid email or password!" });

        return Ok(login);
    }
}