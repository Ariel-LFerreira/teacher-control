using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Mapper;

public class LessonMapper
{
    //DTO -> Entity
    public static Lesson ToEntity(LessonRequestDto lessonRequestDto)
    {
        return new Lesson(
            lessonRequestDto.LessonDate,
            lessonRequestDto.Title,
            lessonRequestDto.Description,
            lessonRequestDto.UserId
            );
    }

    public static LessonResponseDto ToResponse(Lesson lesson)
    {
        return new LessonResponseDto
        {
            LessonDate = lesson.LessonDate,
            Title = lesson.Title,
            Description = lesson.Description,
            Status = lesson.Status.ToString(), // string => APENAS PARA RETORNA O TEXTO NO SWAGGER (TELA)
        };
    }
}