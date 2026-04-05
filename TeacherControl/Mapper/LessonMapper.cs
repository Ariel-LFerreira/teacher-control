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
            lessonRequestDto.Date,
            lessonRequestDto.Title,
            lessonRequestDto.Description,
            lessonRequestDto.UserId);
    }

    public static LessonResponseDto ToResponse(Lesson lesson)
    {
        return new LessonResponseDto
        {
            Date = lesson.Date,
            Title = lesson.Title,
            Description = lesson.Description,
            Status = lesson.Status,
            User = lesson.User == null ? null : new UserResponseDto
            {
                Name = lesson.User.Name,
                Email = lesson.User.Email,
                Role = lesson.User.Role == null ? null : new RoleResponseDto
                {
                    Name = lesson.User.Role.Name,
                    Description = lesson.User.Role.Description
                },
                Status = lesson.User.Status
            },
        };
    }
}