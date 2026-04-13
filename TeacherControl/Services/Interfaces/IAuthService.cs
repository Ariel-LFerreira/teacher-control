using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}