using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace IkariDoTrainingBackend.Services
{
    public class ExecutionService : IExecutionService
    {
        private readonly ApplicationDbContext _context;

        public ExecutionService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Execution> CreateAsync(Execution entity)
        {
            var exercise = await _context.Exercises.FindAsync(entity.ExerciseId);
            if (exercise == null)
                throw new KeyNotFoundException("Exercise not found.");

            _context.Executions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var execution = await _context.Executions.FindAsync(id);
            if (execution == null)
                return false;

            _context.Executions.Remove(execution);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Execution>> GetAllAsync()
        {
            return await _context.Executions.ToListAsync();
        }

        public async Task<Execution> GetByIdAsync(int id)
        {
            return _context.Executions.FirstOrDefault(e => e.Id == id);
        }

        public async Task<Execution> UpdateAsync(Execution entity)
        {
            var existing = await _context.Executions.FindAsync(entity.Id);
            if (existing == null)
                return null;

            existing.Duration = entity.Duration;
            existing.Repetitions = entity.Repetitions;
            existing.Weight = entity.Weight;
            existing.Exercise = entity.Exercise;
            existing.Rating = entity.Rating;
            existing.Comment = entity.Comment;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
