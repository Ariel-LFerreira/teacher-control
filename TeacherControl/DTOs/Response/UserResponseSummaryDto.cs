namespace TeacherControl.DTOs.Response;

public class UserResponseSummaryDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    
    //Comentei pois quero que em tela retorne o texto do enum, ao inves do valor
    //public UserStatus Status { get; set; }
    public string? Status { get; set; }
}