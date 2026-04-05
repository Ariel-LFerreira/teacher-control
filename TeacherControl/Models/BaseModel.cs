namespace TeacherControl.Models;

public class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
}