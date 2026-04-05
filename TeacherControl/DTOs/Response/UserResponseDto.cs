using TeacherControl.Enums;
using TeacherControl.Models;

namespace TeacherControl.DTOs.Response;

public class UserResponseDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public UserStatus Status { get; set; }
    //public Role Role { get; set; }
    public RoleResponseDto Role { get; set; }
    
}