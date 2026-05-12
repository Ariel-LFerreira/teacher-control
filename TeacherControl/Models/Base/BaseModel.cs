namespace TeacherControl.Models.Base;

public abstract class BaseModel
{
    /* Toda entidade precisa se auto validar. Isso evita necessidade de multiplas exceções */
    internal List<string> _errors;

    public IReadOnlyCollection<string> Errors => _errors;
    public abstract bool Validate();
    public Guid Id { get; private set; } = Guid.NewGuid();
    public void ChangeId(Guid id) { Id = id; }
}