using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Services
{
    public interface ISessionService : ICrudService<Session>
    {
        // Falls du noch spezielle Methoden brauchst, hier ergänzen
        // z.B. Task<IEnumerable<Session>> GetByTrainingPlanIdAsync(int planId);
        Task<bool> AddExerciseToSessionAsync(int sessionId, int exerciseId, int sets, int? pauseTime);
        Task<bool> RemoveExerciseFromSessionAsync(int sessionId, int exerciseId);

    }
}
