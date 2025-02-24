using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Services
{
    public interface IUserService : ICrudService<User>
    {
        Task<User> GetUserByNameAsync(string name);
    }
}
