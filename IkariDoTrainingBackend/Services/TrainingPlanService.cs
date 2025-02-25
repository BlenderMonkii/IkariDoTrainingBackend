using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace IkariDoTrainingBackend.Services
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly ApplicationDbContext _context;

        public TrainingPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrainingPlan>> GetAllAsync()
        {
            return await _context.TrainingPlans.ToListAsync();
        }

        public async Task<TrainingPlan> GetByIdAsync(int id)
        {
            return await _context.TrainingPlans
                                 .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<TrainingPlan> CreateAsync(TrainingPlan entity)
        {
            _context.TrainingPlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TrainingPlan> UpdateAsync(TrainingPlan entity)
        {
            // Bsp.: erst aus DB holen
            var existing = await _context.TrainingPlans.FindAsync(entity.Id);
            if (existing == null) return null;

            // Felder, die du ändern willst, übertragen
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.Goal = entity.Goal;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;
            existing.IsPublic = entity.IsPublic;
            // usw.

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan = await _context.TrainingPlans.FindAsync(id);
            if (plan == null) return false;

            _context.TrainingPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TrainingPlan>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.TrainingPlans
                .Where(tp => tp.OwnerId == ownerId)
                .Include(tp => tp.TrainingPlanSessions)
                    .ThenInclude(tps => tps.Session)
                .ToListAsync();
        }


        public async Task<TrainingPlan> AddSessionAsync(int planId, int sessionId)
        {
            var plan = await _context.TrainingPlans
                .Include(tp => tp.TrainingPlanSessions)
                .FirstOrDefaultAsync(tp => tp.Id == planId);

            if (plan == null) return null;

            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null) return null;

            // Check if the association already exists to avoid duplicates
            bool alreadyLinked = plan.TrainingPlanSessions
                .Any(tps => tps.SessionId == sessionId);

            if (!alreadyLinked)
            {
                // Create the new TrainingPlanSession link
                var trainingPlanSession = new TrainingPlanSession
                {
                    TrainingPlanId = planId,
                    SessionId = sessionId
                };

                plan.TrainingPlanSessions.Add(trainingPlanSession);

                await _context.SaveChangesAsync();
            }

            return plan;
        }

    }
}
