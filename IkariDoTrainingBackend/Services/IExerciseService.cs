using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace IkariDoTrainingBackend.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDto>> GetAllAsync(string? type, bool? isPublic, int? ownerId);
        Task<ExerciseBase> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
