using TeacherControl.Enums;
using TeacherControl.Models;

namespace TeacherControl.DTOs.Response;

public class UserResponseDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public RoleResponseDto? Role { get; set; }
    public ICollection<LessonResponseDto>? Lessons { get; set; }
    
}