using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
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
        var login = await userService.Login(loginRequestDto);

        if (login == null)
            return Unauthorized(new { menssage = "Email ou senha invalido" });

        return Ok(login);
    }
    
    [HttpPost("/AddUser")]
    public async Task<ActionResult<UserResponseDto>> Post([FromBody] UserRequestDto userRequestDto)
    {
        //==================================
        //UTILIZANDO O ADD DO BASESERVICE
        //==================================
        var userResponse = await userService.Add(userRequestDto);
        return Ok(userResponse);
    }
    
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmail(email);
        return Ok(user);
    }

    [HttpPut("/UpdateUser/{id}")]
    public async Task<ActionResult<UserResponseDto>> Put(Guid id, [FromBody] UserRequestDto userRequestDto)
    {
        var updateUser = await userService.Update(id, userRequestDto);
        return Ok(updateUser);
    }
    
    [Authorize(Roles="Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.Remove(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAll()
    {
        var listUsers = await userService.GetAll();
        return Ok(listUsers);
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<List<UserResponseDto>>> GetById(Guid id)
    {
        var user = await userService.GetById(id);
        return Ok(user);
    }

}