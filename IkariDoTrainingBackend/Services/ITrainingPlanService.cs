﻿using IkariDoTrainingBackend.Models;

namespace IkariDoTrainingBackend.Services
{
    public interface ITrainingPlanService : ICrudService<TrainingPlan>
    {
        Task<IEnumerable<TrainingPlan>> GetByOwnerIdAsync(int ownerId);
        Task<TrainingPlan> AddSessionAsync(int planId, int sessionId);
    }
}
