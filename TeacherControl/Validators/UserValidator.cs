using FluentValidation;
using TeacherControl.Models;

namespace TeacherControl.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must have a maximum of 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email is invalid")
             /*.Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")  // Regex manual de email (COMENTADO)
            .WithMessage("email is invalid")*/
            .MaximumLength(200)
            .WithMessage("Email must have a maximum of 200 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must have at least 6 characters");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status is invalid");
    }
}


