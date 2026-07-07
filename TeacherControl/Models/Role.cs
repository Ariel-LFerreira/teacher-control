using TeacherControl.Extensions;
using TeacherControl.Models.Base;

namespace TeacherControl.Models;

public class Role : BaseModel
{
    public Role(String name, string description)
    {
        Name = name;
        Description = description;

        Validate();
    }
    
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public ICollection<User>? Users { get; private set; }
    public void SetName(string name) { Name = name; }
    public void SetDescription(string description) { Description = description; }
    
    public override bool Validate()
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(Description))
            errors.Add("Description is invalid");

        if (errors.Any())
            throw new DomainException(errors);

        return true;
    }
}