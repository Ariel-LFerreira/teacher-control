using TeacherControl.Enums;

namespace TeacherControl.Models;

public class User : BaseModel
{
    public User(string email, string password, string name, Guid roleId)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("Name is Invalid");
        
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("Password is Invalid");
            
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Name is Invalid");
        
        if (roleId == Guid.Empty)
            throw new Exception("Role Id is Invalid");
        
        Email = email;
        Password = password;
        Name = name;
        RoleId = roleId;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("email is invalid!");
        
        Email = email;
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("Password is invalid!");

        Password = password;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Name is invalid!");

        Name = name;
    }

    public void SetStatus(UserStatus status)
    {
        if (!Enum.IsDefined(typeof(UserStatus), status))
            throw new Exception("Status is invalid!");

        Status = status;
    }

    public void SetRoleId(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new Exception("Role Id is Invalid");

        RoleId = roleId;
    }

    public string Email { get; private set; }
    public string Password { get;private set; }
    public string Name { get; private set; }
    public UserStatus Status { get; private set; }
    // FK
    public Guid RoleId { get; private set; }

    // Navegação
    public Role? Role { get; private set; }
    
    public ICollection<Lesson> Lessons { get; private set; }  = new List<Lesson>();
}