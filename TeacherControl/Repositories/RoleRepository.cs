using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Repositories;

public class RoleRepository(AppDbContext context) : BaseRepository<Role>(context), IRoleRepository
{
    
    
}