using TeacherControl.Models;

namespace TeacherControl.DTOs.Response;

public class LoginResponseDto
{
    public Guid Id { get; set; } // APENAS PARA FACILITAR OS TESTES, REMOVER PARA LIBERAÇÃO
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public RoleResponseDto? role { get; set; }
    
}