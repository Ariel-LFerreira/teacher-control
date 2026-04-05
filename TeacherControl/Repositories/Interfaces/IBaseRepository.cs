using TeacherControl.Models;

namespace TeacherControl.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<T> Remove(Guid id);
    Task<T> GetById(Guid id);
    Task<List<T>> GetAll();
}