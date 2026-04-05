namespace TeacherControl.Models;

public class Role : BaseModel
{
    public Role(String name, string description)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new Exception("Name is Invalid!");

        if (string.IsNullOrWhiteSpace(description))
            throw new Exception("Description is Invalid!!!!");
        
        Name = name;
        Description = description;

    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Name is invalid!");
        
        Name = name;
    }

    public void SetDescription(string description)
    {   
        if (string.IsNullOrWhiteSpace(description))
            throw new Exception("Description is Invalid!");

        Description = description;
    }
    
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public ICollection<User>? Users { get; private set; }
}