using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        //return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        return await context.Users.Include(u => u.Role).
            AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<List<User>> GetAll()
    {
        return await context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetById(Guid id)
    {
        return await context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}