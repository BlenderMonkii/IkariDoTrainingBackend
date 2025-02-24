using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace IkariDoTrainingBackend.Services
{
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            // Asynchrones Laden
            return await _context.Sessions
                .Include(s => s.TrainingPlans)
                .Include(s => s.SessionExercises)
                .ToListAsync();
        }

        public async Task<Session> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.TrainingPlans)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Session> CreateAsync(Session entity)
        {
            // 1. Falls entity.TrainingPlans existiert, attach sie (damit EF weiß, dass sie schon existieren).
            foreach (var tp in entity.TrainingPlans)
            {
                if (_context.Entry(tp).State == EntityState.Detached)
                {
                    // Wenn TrainingPlan nicht getrackt wird, anfügen (Attach).
                    _context.TrainingPlans.Attach(tp);
                }
            }

            // 2. Session hinzufügen
            _context.Sessions.Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Session> UpdateAsync(Session entity)
        {
            var existing = await _context.Sessions
                .Include(s => s.TrainingPlans)
                .FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (existing == null) return null;

            // Felder übernehmen
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.SessionDate = entity.SessionDate;
            existing.Duration = entity.Duration;
            existing.IsPublic = entity.IsPublic;
            existing.OwnerId = entity.OwnerId;

            // 1. Alte Verknüpfungen entfernen
            existing.TrainingPlans.Clear();

            // 2. Neue zuweisen
            foreach (var tp in entity.TrainingPlans)
            {
                // Falls EF diesen Trainingsplan noch nicht trackt, anhängen.
                if (_context.Entry(tp).State == EntityState.Detached)
                {
                    _context.TrainingPlans.Attach(tp);
                }

                existing.TrainingPlans.Add(tp);
            }
            // Speichern
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddExerciseToSessionAsync(int sessionId, int exerciseId, int sets, int? pauseTime)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (session == null || exercise == null)
                return false;

            var sessionExercise = new SessionExercise
            {
                SessionId = sessionId,
                ExerciseId = exerciseId,
                Sets = sets,
                PauseTime = pauseTime
            };

            _context.SessionExercises.Add(sessionExercise);
            await _context.SaveChangesAsync();
            return true;
        }

        // Exercise aus einer Session entfernen
        public async Task<bool> RemoveExerciseFromSessionAsync(int sessionId, int exerciseId)
        {
            var sessionExercise = await _context.SessionExercises
                .FirstOrDefaultAsync(se => se.SessionId == sessionId && se.ExerciseId == exerciseId);

            if (sessionExercise == null)
                return false;

            _context.SessionExercises.Remove(sessionExercise);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
