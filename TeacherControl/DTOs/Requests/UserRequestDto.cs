namespace TeacherControl.DTOs.Requests;

public class UserRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}