using TeacherControl.Models;

namespace TeacherControl.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken (User user);
}