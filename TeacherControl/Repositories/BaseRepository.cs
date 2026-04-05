using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Repositories;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : BaseModel
{
    public virtual async Task<T> Add(T entity)
    {
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> Remove(Guid id)
    {
        var entity = await GetById(id);
        
        if (entity == null)
            throw new Exception("Entity not found");
        
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await context.Set<T>().AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<List<T>> GetAll()
    {
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }
}