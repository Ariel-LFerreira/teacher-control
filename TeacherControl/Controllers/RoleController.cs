using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(Roles="student")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    [HttpPost("/AddRole")]
    public async Task<ActionResult<Role>> Post([FromBody] RoleRequestDto roleRequestDto)
    {
        var roleResponse = await roleService.Add(roleRequestDto);
        return Ok(roleResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Role>> Put(Guid id, [FromBody] RoleRequestDto roleRequestDto)
    {
        var roleUpdate = await roleService.Update(id, roleRequestDto);
        return Ok(roleUpdate);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await roleService.Remove(id);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<RoleResponseDto>>> GetAll()
    {
        var listRoles = await roleService.GetAll();
        return Ok(listRoles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponseDto>> GetById(Guid id)
    {
        var role = await roleService.GetById(id);
        return Ok(role);
    }
    
}