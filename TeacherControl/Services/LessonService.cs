using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class LessonService(
                ILessonRepository lessonRepository,
                IUserRepository userRepository) : ILessonService
{
    public async Task<LessonResponseDto> Add(LessonRequestDto lessonRequestDto)
    {
        //Verifica se o User ID é valido(SE EXISTE O USUÁRIO)
        var ExistUser = userRepository.GetById(lessonRequestDto.UserId);

        if (ExistUser is null)
            throw new UnauthorizedAccessException("Invalid User!");
        
        var lesson = LessonMapper.ToEntity(lessonRequestDto); 
        
        await lessonRepository.Add(lesson);

        var lessonCreated = await lessonRepository.GetById(lesson.Id);
    
        //Está retornando o objeto Lesson que acabou de ser criado (SEM O ROLE) POR ISSO REALIZO O GETBYID.
        return LessonMapper.ToResponse(lessonCreated);
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