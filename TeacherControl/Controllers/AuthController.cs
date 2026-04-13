using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService): ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() =>
        Ok(new {
            id    = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            email = User.FindFirst(ClaimTypes.Email)?.Value,
            role  = User.FindFirst(ClaimTypes.Role)?.Value 
        });
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var login = await authService.Login(loginRequestDto);

        if (login == null)
            return Unauthorized(new { menssage = "Invalid email or password!" });

        return Ok(login);
    }
}