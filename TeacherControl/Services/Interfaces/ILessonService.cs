using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Services.Interfaces;

public interface ILessonService
{
    Task<LessonResponseDto> Add(LessonRequestDto lessonRequestDto);
    Task<LessonResponseDto> Update(Guid id, LessonRequestDto lessonRequestDto);
    Task Remove(Guid id);
    Task<LessonResponseDto> GetById(Guid id);
    Task<List<LessonResponseDto>> GetAll();
}