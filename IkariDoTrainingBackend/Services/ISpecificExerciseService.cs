using IkariDoTrainingBackend.Models.Exercises;

namespace IkariDoTrainingBackend.Services
{
    public interface ISpecificExerciseService<T> where T : ExerciseBase
    {
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T exercise);
        Task<bool> UpdateAsync(T exercise);
    }
}
