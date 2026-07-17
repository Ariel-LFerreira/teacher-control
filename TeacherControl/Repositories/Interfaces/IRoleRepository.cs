using TeacherControl.Models;

namespace TeacherControl.Repositories.Interfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetRoleByName(string name);
}