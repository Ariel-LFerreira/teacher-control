using TeacherControl.Models;

namespace TeacherControl.DTOs.Requests;

/*
 * Isso é um DTO puro (anêmico)
 * Não tem construtor
 * Não tem validação
 * Não lança exception
 */

public class LessonRequestDto
{
    public DateOnly LessonDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}