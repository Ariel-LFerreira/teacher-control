using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Extensions;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<RoleResponseDto> Add(RoleRequestDto roleRequestDto)
    {
        var roleAlredyExists = await roleRepository.GetRoleByName(roleRequestDto.Name);

        if (roleAlredyExists != null)
            throw new DomainException("Role already exists");

        var role = RoleMapper.ToEntity(roleRequestDto);

        await roleRepository.Add(role);

        return RoleMapper.ToResponse(role);
    }

    public async Task<RoleResponseDto> Update(Guid id, RoleRequestDto roleRequestDto)
    {
        var roleFound = await roleRepository.GetById(id);

        if (roleFound == null)
            throw new Exception("Role is not Found!!!!");
        
        roleFound.SetName(roleRequestDto.Name);
        roleFound.SetDescription(roleRequestDto.Description);
        
        await roleRepository.Update(roleFound);

        return RoleMapper.ToResponse(roleFound);
    }

    public async Task Remove(Guid id)
    {
        var role = await roleRepository.GetById(id);

        if (role == null)
            throw new Exception("Role não encontrada!");

        if (role.Users != null && role.Users.Any())
            throw new Exception("Role possui usuários vinculados");

        await roleRepository.Remove(id);
    }

    public async Task<RoleResponseDto> GetById(Guid id)
    {
        var role = await roleRepository.GetById(id);

        if(role == null)
            throw new Exception("Role not Found!");
        
        return RoleMapper.ToResponse((role));
    }

    public async Task<List<RoleResponseDto>> GetAll()
    {
        var listRoles = await roleRepository.GetAll();

        if (listRoles == null)
            throw new Exception("No data found.");

        return listRoles.Select(RoleMapper.ToResponse).ToList();
    }
}