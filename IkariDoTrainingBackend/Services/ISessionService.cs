using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Services
{
    public interface ISessionService : ICrudService<SessionDto>
    {
        Task<IEnumerable<SessionDto>> GetAllByOwnerIdAsync(int ownerId);
        Task<SessionDto> GetByIdAndOwnerAsync(int sessionId, int ownerId);
        Task<bool> AddExerciseToSessionAsync(int sessionId, int exerciseId, int sets, int? pauseTime);
        Task<bool> RemoveExerciseFromSessionAsync(int sessionId, int exerciseId);

    }
}
