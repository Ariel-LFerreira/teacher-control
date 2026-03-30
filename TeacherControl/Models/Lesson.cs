using TeacherControl.Enums;

namespace TeacherControl.Models;

public class Lesson
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public LessonStatus Status { get; set; }
    
    public Guid User { get; set; }
}