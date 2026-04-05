using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Services.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto> Add(RoleRequestDto roleRequestDto);
    Task<RoleResponseDto> Update(Guid id, RoleRequestDto roleRequest);
    Task Remove(Guid id);
    Task<RoleResponseDto> GetById(Guid id);
    Task<List<RoleResponseDto>> GetAll();
}