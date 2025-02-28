using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Dtos;
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

        public async Task<IEnumerable<SessionDto>> GetAllAsync()
        {
            // Asynchrones Laden
            var sessions = await _context.Sessions
                //.Include(s => s.TrainingPlanSessions)
                .Include(s => s.SessionExercises)
                .ThenInclude(se => se.Exercise)
                .ToListAsync();

            // Konvertiere zu SessionDto
            var sessionDtos = sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                SessionDate = s.SessionDate,
                Duration = s.Duration,
                IsPublic = s.IsPublic,
                OwnerId = s.OwnerId,
                Type = s.Type,
                SessionExercises = s.SessionExercises.ToList()
            });

            return sessionDtos;
        }


        public async Task<SessionDto> GetByIdAsync(int id)
        {
            var session = await _context.Sessions
                .Include(s => s.TrainingPlanSessions)
                .Include(s => s.SessionExercises)
                .ThenInclude(se => se.Exercise)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null) return null;

            return new SessionDto
            {
                Id = session.Id,
                Name = session.Name,
                Description = session.Description,
                SessionDate = session.SessionDate,
                Duration = session.Duration,
                IsPublic = session.IsPublic,
                OwnerId = session.OwnerId,
                Type = session.Type,
                SessionExercises = session.SessionExercises.ToList()
            };
        }

        public async Task<SessionDto> CreateAsync(SessionDto entity)
        {
            var session = new Session
            {
                Name = entity.Name,
                Description = entity.Description,
                SessionDate = entity.SessionDate,
                Duration = entity.Duration,
                IsPublic = entity.IsPublic,
                OwnerId = entity.OwnerId,
                Type = entity.Type,
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            // 1. Attach existing TrainingPlans and create TrainingPlanSession entries
            //foreach (var tpSession in session.TrainingPlanSessions)
            //{
            //    if (_context.Entry(tpSession.TrainingPlan).State == EntityState.Detached)
            //    {
            //        // Attach the existing TrainingPlan
            //        _context.TrainingPlans.Attach(tpSession.TrainingPlan);
            //    }
            //}

            return new SessionDto
            {
                Id = session.Id,
                Name = session.Name,
                Description = session.Description,
                SessionDate = session.SessionDate,
                Duration = session.Duration,
                IsPublic = session.IsPublic,
                OwnerId = session.OwnerId,
                Type = session.Type
            };
        }


        public async Task<SessionDto> UpdateAsync(SessionDto entity)
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
            existing.Type = entity.Type;

            // 2. Remove existing TrainingPlanSessions
            _context.RemoveRange(existing.TrainingPlanSessions);

            // 3. Add new TrainingPlanSessions
            //foreach (var tpSession in entity.TrainingPlanSessions)
            //{
            //    if (_context.Entry(tpSession.TrainingPlan).State == EntityState.Detached)
            //    {
            //        _context.TrainingPlans.Attach(tpSession.TrainingPlan);
            //    }

            //    existing.TrainingPlanSessions.Add(new TrainingPlanSession
            //    {
            //        TrainingPlanId = tpSession.TrainingPlanId,
            //        SessionId = existing.Id
            //    });
            //}

            await _context.SaveChangesAsync();

            // Konvertiere zurück zu SessionDto
            return new SessionDto
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                SessionDate = existing.SessionDate,
                Duration = existing.Duration,
                IsPublic = existing.IsPublic,
                OwnerId = existing.OwnerId,
                Type = existing.Type,
                SessionExercises = existing.SessionExercises.ToList()
            };
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
                PauseTime = pauseTime,
                Exercise = exercise,
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

        public async Task<IEnumerable<SessionDto>> GetAllByOwnerIdAsync(int ownerId)
        {
            var sessions = await _context.Sessions
                .Where(s => s.OwnerId == ownerId) // || s.IsPublic == true
                .Include(s => s.SessionExercises)
                .ToListAsync();

            var sessionDtos = sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                SessionDate = s.SessionDate,
                Duration = s.Duration,
                IsPublic = s.IsPublic,
                OwnerId = s.OwnerId,
                Type = s.Type,
                SessionExercises = s.SessionExercises.ToList()
            });

            return sessionDtos;
        }

        public async Task<SessionDto> GetByIdAndOwnerAsync(int sessionId, int ownerId)
        {
            var session = await _context.Sessions
                .Include(s => s.SessionExercises)
                .FirstOrDefaultAsync(s => s.Id == sessionId && (s.OwnerId == ownerId || s.IsPublic));

            if (session == null) return null;

            return new SessionDto
            {
                Id = session.Id,
                Name = session.Name,
                Description = session.Description,
                SessionDate = session.SessionDate,
                Duration = session.Duration,
                IsPublic = session.IsPublic,
                OwnerId = session.OwnerId,
                Type = session.Type,
                SessionExercises = session.SessionExercises.ToList()
            };
        }
    }
}
