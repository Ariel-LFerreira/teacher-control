using TeacherControl.Enums;

namespace TeacherControl.DTOs.Response;

public class LessonResponseDto
{
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public LessonStatus Status { get; set; }
    public UserResponseDto User { get; set; }
    
}