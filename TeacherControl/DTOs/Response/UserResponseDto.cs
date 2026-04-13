using TeacherControl.Enums;
using TeacherControl.Models;

namespace TeacherControl.DTOs.Response;

public class UserResponseDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    
    //Comentei pois quero que em tela retorne o texto do enum, ao inves do valor
    //public UserStatus Status { get; set; }
    public string? Status { get; set; }

    //public Role Role { get; set; }
    public RoleResponseDto? Role { get; set; }
    public ICollection<LessonResponseDto>? Lessons { get; set; }
    
}