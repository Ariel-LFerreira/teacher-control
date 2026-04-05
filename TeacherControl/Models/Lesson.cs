using TeacherControl.Enums;

namespace TeacherControl.Models;

public class Lesson : BaseModel
{
    /*
     * “O EF Core precisa de um construtor sem parâmetros para materializar entidades ao carregar dados do banco.
     * Usamos protected para permitir acesso via reflection pelo EF,
     * mas impedir uso indevido pela aplicação.
     */
    protected Lesson() { }
    public Lesson(DateOnly date, string title, string description, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new Exception("Title is invalid");

        if (string.IsNullOrWhiteSpace(description))
            throw new Exception("Description is invalid");

        if (userId == Guid.Empty)
            throw new Exception("UserId is invalid"); 
        
        Date = date;
        Title = title;
        Description = description;
        UserId = userId;
    }

    public DateOnly Date { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public LessonStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new Exception("Title is invalid");

        Title = title;
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new Exception("Description is invalid");

        Description = description;
    }

    public void SetDate(DateOnly date)
    {
        Date = date;
    }

    public void SetStatus(LessonStatus status)
    {
        Status = status;
    }
}