using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Models.Base;

namespace TeacherControl.Models;

public class User : BaseModel
{
    protected User(){}

    public User(string email, string password, string name, Guid roleId)
    {
        SetEmail(email);
        SetPassword(password);
        SetName(name);
        SetRoleId(roleId);
        SetStatus(UserStatus.Active);
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Name { get; private set; }
    public UserStatus Status { get; private set; }

    // FK
    public Guid RoleId { get; private set; }

    // Navegação
    public Role? Role { get; private set; }

    public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();


    // =========================================
    // MÉTODOS DE DOMÍNIO
    // =========================================

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        Email = email.ToLower().Trim();
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("Password cannot be empty");

        Password = password;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");

        Name = name.Trim();
    }

    public void SetRoleId(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new DomainException("RoleId is invalid");

        RoleId = roleId;
    }

    public void SetStatus(UserStatus status)
    {
        if (!Enum.IsDefined(typeof(UserStatus), status))
            throw new DomainException("Status is invalid");

        Status = status;
    }

    public override bool Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(Password))
            errors.Add("Password cannot be empty");

        if (RoleId == Guid.Empty)
            errors.Add("RoleId is invalid");

        if (!Enum.IsDefined(typeof(UserStatus), Status))
            errors.Add("Status is invalid");

        if (errors.Any())
            throw new DomainException(errors);

        return true;
    }
}