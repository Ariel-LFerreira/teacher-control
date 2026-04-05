using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    public async Task<LessonResponseDto> Add(LessonRequestDto lessonRequestDto)
    {
        var lesson = LessonMapper.ToEntity(new LessonRequestDto()); 
        
        await lessonRepository.Add(lesson);

        return LessonMapper.ToResponse(lesson);
    }

    public async Task<LessonResponseDto> Update(Guid id, LessonRequestDto lessonRequestDto)
    {
        var lessonFound = await lessonRepository.GetById(id);

        if (lessonFound == null)
            throw new Exception("Lesson is not found!!!!");
        
        lessonFound.SetDate(lessonRequestDto.Date);
        lessonFound.SetTitle(lessonRequestDto.Title);
        lessonFound.SetDescription(lessonRequestDto.Description);
        
        await lessonRepository.Update(lessonFound);

        return LessonMapper.ToResponse(lessonFound);
    }

    public async Task Remove(Guid id)
    {
        await lessonRepository.Remove(id);
    }

    public async Task<LessonResponseDto> GetById(Guid id)
    {
        var lessonFound = await lessonRepository.GetById(id);

        if (lessonFound == null)
            throw new Exception("Lesson is not found!");
        
        return LessonMapper.ToResponse(lessonFound);
    }

    public async Task<List<LessonResponseDto>> GetAll()
    {
        var listLessons = await lessonRepository.GetAll();

        if (listLessons == null)
            throw new Exception("Lessons is not found!");
        
        return listLessons.Select(LessonMapper.ToResponse).ToList();
    }
}