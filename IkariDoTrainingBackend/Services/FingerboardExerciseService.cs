using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Models.Exercises;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IkariDoTrainingBackend.Services
{
    public class FingerboardExerciseService : ISpecificExerciseService<FingerboardExercise>
    {
        private readonly ApplicationDbContext _context;

        public FingerboardExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FingerboardExercise>> GetAllAsync()
        {
            return await _context.FingerboardExercises.Include(e => e.Owner).Include(e => e.Timer).ToListAsync();
        }

        public async Task<FingerboardExercise> CreateAsync(FingerboardExercise exercise)
        {
            _context.FingerboardExercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }


        public async Task<bool> UpdateAsync(FingerboardExercise exercise)
        {
            var existingExercise = await _context.FingerboardExercises.FindAsync(exercise.Id);
            if (existingExercise == null) return false;

            _context.Entry(existingExercise).CurrentValues.SetValues(exercise);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
