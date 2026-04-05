using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Mapper;

public class RoleMapper
{
    //DTO -> Entity

    public static Role ToEntity(RoleRequestDto roleRequestDto)
    {
        return new Role(
            roleRequestDto.Name,
            roleRequestDto.Description
        );

    }

    public static RoleResponseDto ToResponse(Role role)
    {
        return new RoleResponseDto
        {
            Name = role.Name,
            Description = role.Description
        };
    }
}