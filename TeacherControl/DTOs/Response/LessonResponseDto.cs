using TeacherControl.Enums;

namespace TeacherControl.DTOs.Response;

public class LessonResponseDto
{
    public DateOnly LessonDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Status { get; set; }
}