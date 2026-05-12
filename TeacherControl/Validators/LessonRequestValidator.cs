using FluentValidation;
using TeacherControl.DTOs.Requests;

namespace TeacherControl.Validators;

public class LessonRequestValidator : AbstractValidator<LessonRequestDto>
{
    public LessonRequestValidator()
    {
       RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}