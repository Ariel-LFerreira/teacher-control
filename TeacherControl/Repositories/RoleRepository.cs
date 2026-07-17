using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Repositories;

public class RoleRepository(AppDbContext context) : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<Role?> GetRoleByName(string name)
    {
        var t = await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name);
        return t;
    }
}