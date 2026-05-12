using TeacherControl.Models;

namespace TeacherControl.DTOs.Requests;

public class LessonRequestDto
{
    public DateOnly LessonDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}