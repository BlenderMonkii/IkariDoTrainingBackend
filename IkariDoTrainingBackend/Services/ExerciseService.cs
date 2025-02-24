using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IkariDoTrainingBackend.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ApplicationDbContext _context;

        public ExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseDto>> GetAllAsync(string? type, bool? isPublic, int? ownerId)
        {
            IQueryable<ExerciseBase> query = _context.Exercises;

            if (!string.IsNullOrEmpty(type))
            {
                switch (type.ToLower())
                {
                    case "fingerboard":
                        query = query.OfType<FingerboardExercise>();
                        break;
                    default:
                        throw new ArgumentException($"Unbekannter Exercise-Typ: {type}");
                }
            }

            if (isPublic.HasValue)
                query = query.Where(e => e.IsPublic == isPublic.Value);

            if (ownerId.HasValue)
                query = query.Where(e => e.OwnerId == ownerId.Value);

            var exerciseDtos = await query.Select(e => new ExerciseDto
            {
                Id = e.Id,
                OwnerId = e.OwnerId,
                Name = e.Name,
                Description = e.Description,
                Duration = e.Duration,
                IsPublic = e.IsPublic,
                Location = e.Location,
                Repetitions = e.Repetitions,
                ExerciseType = e.GetType().Name, // Holt den Typnamen (z.B. "FingerboardExercise")
                Executions = e.Executions.ToList()
            }).ToListAsync();

            return exerciseDtos;
        }
    

        public async Task<ExerciseBase> GetByIdAsync(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return false;

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
