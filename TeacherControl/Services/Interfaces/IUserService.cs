using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Services.Interfaces;

public interface IUserService: IBaseService<UserRequestDto,UserResponseDto>
{
    Task<UserResponseDto> Create(UserRequestDto userRequestDto);
    Task<UserResponseDto> Update (Guid id, UserRequestDto userRequest);
    Task Remove(Guid id);
    Task<UserResponseDto> GetById(Guid id);
    Task<List<UserResponseDto>> GetAll();
    Task<UserResponseDto?> GetUserByEmail(string email);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}