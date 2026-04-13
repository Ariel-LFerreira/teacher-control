using TeacherControl.Enums;

namespace TeacherControl.DTOs.Response;

public class LessonResponseDto
{
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    //COMENTADO: EM PROD: LIBERAR DO TIPO ENUM, MAS PARA FINS VISUAIS ESTOU RETORNANDO STRING
    //public LessonStatus Status { get; set; } 
    public string? Status { get; set; }
    //public UserResponseDto User { get; set; }
}