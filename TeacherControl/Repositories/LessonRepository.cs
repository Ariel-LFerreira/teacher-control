using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Repositories;

public class LessonRepository(AppDbContext context) : BaseRepository<Lesson>(context), ILessonRepository
{
    public async Task<List<Lesson>> GetAll()
    {
        return await context.Lessons
            .Include(l => l.User)
            .AsNoTracking()
            .ToListAsync();
    }

    // public async Task<Lesson?> GetById(Guid id)
    // {
    //     return await context.Lessons
    //         .Include(l => l.User)
    //         .ThenInclude(u => u.Role)
    //         .AsNoTracking()
    //         .FirstOrDefaultAsync(l => l.Id == id);
    // }
    
}

