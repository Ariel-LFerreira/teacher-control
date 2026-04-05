namespace TeacherControl.DTOs.Requests;

public class UserRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public Guid RoleId { get; set; }
}