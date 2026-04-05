using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Mapper;

public static class UserMapper
{
    //DTO => Entity
    public static User ToEntity(UserRequestDto userRequestDto)
    {
        return new User(
                    userRequestDto.Email, 
                    userRequestDto.Password, 
                    userRequestDto.Name, 
                    userRequestDto.RoleId
        );
    }
    
    //Entity => DTO
    public static UserResponseDto ToResponse(User user)
    {
        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            //Role = user.Role, 
            Role = user.Role == null ? null : new RoleResponseDto
            {
                Name = user.Role.Name,
                Description = user.Role.Description
            },
            Status = user.Status
        };
    }
    
}