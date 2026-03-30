using TeacherControl.Enums;

namespace TeacherControl.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public UserStatus status { get; set; }
    public string Role { get; set; } 
    
    public ICollection<Lesson> Lessons { get; set; }
}