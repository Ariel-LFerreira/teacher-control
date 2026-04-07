using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories;
using TeacherControl.Services;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("/AddUser")]
    public async Task<ActionResult<UserResponseDto>> Post([FromBody] UserRequestDto userRequestDto)
    {
        var userResponse = await userService.Add(userRequestDto);
        return Ok(userResponse);
    }

    [HttpPut("/UpdateUser/{id}")]
    public async Task<ActionResult<UserResponseDto>> Put(Guid id, [FromBody] UserRequestDto userRequestDto)
    {
        var updateUser = await userService.Update(id, userRequestDto);
        return Ok(updateUser);
    }

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

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmail(email);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Auth(LoginRequestDto loginRequestDto)
    {
        var login = await userService.Auth(loginRequestDto);

        if (login == null)
            return Unauthorized(new { menssage = "Email ou senha invalido" });

        return Ok(login);
    }

}