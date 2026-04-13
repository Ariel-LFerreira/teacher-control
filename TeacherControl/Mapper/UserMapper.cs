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
    
    //Entity => DTO Basic info
    public static UserResponseSummaryDto ToSumaryResponse(User user)
    {
        return new UserResponseSummaryDto
        {
            Name = user.Name,
            Email = user.Email,
            Status = user.Status.ToString(), // string => APENAS PARA RETORNA O TEXTO NO SWAGGER (TELA)
        };
    }
    
    //Entity => DTO
    public static UserResponseDto ToResponse(User user)
    {
        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Status = user.Status.ToString(), // string => APENAS PARA RETORNA O TEXTO NO SWAGGER (TELA)
            Lessons = user.Lessons == null 
                                    ? null : 
                                    user.Lessons.Select(l => new LessonResponseDto
                                    {
                                        Title = l.Title,
                                        Status = l.Status.ToString(),
                                        Description = l.Description
                                        
                                        
                                    }).ToList(),
            Role = user.Role == null ? null : new RoleResponseDto
            {
                Name = user.Role.Name,
                Description = user.Role.Description
            },
            
        };
    }
    
}