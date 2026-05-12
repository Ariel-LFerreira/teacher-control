namespace TeacherControl.Extensions;

public class DomainException : Exception
{
    private readonly List<string> _errors = new();

    public IReadOnlyCollection<string> Errors => _errors;

    public DomainException(string error)
        : base("One or more validation errors occurred.")
    {
        _errors.Add(error);
    }

    public DomainException(List<string> errors)
        : base("One or more validation errors occurred.")
    {
        _errors = errors;
    }

    public DomainException(string message, Exception inner)
        : base(message, inner)
    {
    }
}