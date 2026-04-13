using TeacherControl.DTOs.Response;
using TeacherControl.Models;

namespace TeacherControl.Repositories.Interfaces;

public interface IUserRepository: IBaseRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<List<User>> GetAllFull();
}