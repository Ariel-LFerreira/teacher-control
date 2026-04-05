using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> Add(UserRequestDto userRequestDto);
    Task<UserResponseDto> Update (Guid id, UserRequestDto userRequest);
    Task Remove(Guid id);
    Task<UserResponseDto> GetById(Guid id);
    Task<List<UserResponseDto>> GetAll();
    Task<UserResponseDto?> GetUserByEmail(string email);
}