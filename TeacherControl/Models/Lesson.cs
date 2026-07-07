using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Models.Base;

namespace TeacherControl.Models;

public class Lesson : BaseModel
{
    protected Lesson() { }
    
    public Lesson(DateOnly lessonDate, string title, string description, Guid userId )
    {
        LessonDate = lessonDate;
        Title = title;
        Description = description;
        UserId = userId;

        Validate();
    }

    public DateOnly LessonDate { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public LessonStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public void SetTitle(string title) { Title = title; }
    public void SetDescription(string description) { Description = description; }
    public void SetDate(DateOnly date) { LessonDate = date; }
    public void SetStatus(LessonStatus status) { Status = status; }

    public override bool Validate()
    {
        var errors = new List<string>();
        
        if (LessonDate == default)
            errors.Add("Lesson Date is invalid");
        
        if (string.IsNullOrWhiteSpace(Title))
            errors.Add("Title cannot be empty");

        if (string.IsNullOrWhiteSpace(Description))
            errors.Add("Description is invalid");
        
        if (UserId == Guid.Empty)
            errors.Add("UserId is invalid");

        if (errors.Any())
            throw new DomainException(errors);

        return true;
    }
    
}