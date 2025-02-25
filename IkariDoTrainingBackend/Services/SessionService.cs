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
                .Include(s => s.TrainingPlanSessions)
                .Include(s => s.SessionExercises)
                .ToListAsync();
        }

        public async Task<Session> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.TrainingPlanSessions)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Session> CreateAsync(Session entity)
        {
            // 1. Attach existing TrainingPlans and create TrainingPlanSession entries
            foreach (var tpSession in entity.TrainingPlanSessions)
            {
                if (_context.Entry(tpSession.TrainingPlan).State == EntityState.Detached)
                {
                    // Attach the existing TrainingPlan
                    _context.TrainingPlans.Attach(tpSession.TrainingPlan);
                }
            }

            // 2. Add the new Session along with its TrainingPlanSessions
            _context.Sessions.Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Session> UpdateAsync(Session entity)
        {
            var existing = await _context.Sessions
                .Include(s => s.TrainingPlanSessions)
                .FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (existing == null) return null;

            // 1. Update basic fields
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.SessionDate = entity.SessionDate;
            existing.Duration = entity.Duration;
            existing.IsPublic = entity.IsPublic;
            existing.OwnerId = entity.OwnerId;

            // 2. Remove existing TrainingPlanSessions
            _context.RemoveRange(existing.TrainingPlanSessions);

            // 3. Add new TrainingPlanSessions
            foreach (var tpSession in entity.TrainingPlanSessions)
            {
                if (_context.Entry(tpSession.TrainingPlan).State == EntityState.Detached)
                {
                    _context.TrainingPlans.Attach(tpSession.TrainingPlan);
                }

                existing.TrainingPlanSessions.Add(new TrainingPlanSession
                {
                    TrainingPlanId = tpSession.TrainingPlanId,
                    SessionId = existing.Id
                });
            }

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
