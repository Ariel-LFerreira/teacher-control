using FluentValidation;
using TeacherControl.DTOs.Requests;

namespace TeacherControl.Validators;

public class RoleRequestValidator : AbstractValidator<RoleRequestDto>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name Role is required")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}