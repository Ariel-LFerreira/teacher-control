namespace TeacherControl.Models;

public abstract class BaseModel
{
    public Guid Id { get; private set; } = Guid.NewGuid();


    public void ChangeID(Guid id)
    {
        Id = id;
    }
}