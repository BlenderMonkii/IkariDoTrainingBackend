using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingPlanController : ControllerBase
    {
        private readonly ITrainingPlanService _trainingPlanService;

        public TrainingPlanController(ITrainingPlanService trainingPlanService)
        {
            _trainingPlanService = trainingPlanService;
        }

        // GET: api/TrainingPlan
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingPlan>>> GetAll()
        {
            var plans = await _trainingPlanService.GetAllAsync();
            return Ok(plans);
        }

        // GET: api/TrainingPlan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingPlan>> Get(int id)
        {
            var plan = await _trainingPlanService.GetByIdAsync(id);
            if (plan == null) return NotFound();

            return Ok(plan);
        }

        // POST: api/TrainingPlan
        [HttpPost]
        public async Task<ActionResult<TrainingPlan>> Create([FromBody] TrainingPlan plan)
        {
            if (plan == null) return BadRequest();

            var created = await _trainingPlanService.CreateAsync(plan);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT: api/TrainingPlan/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TrainingPlan plan)
        {
            if (id != plan.Id) return BadRequest("ID mismatch.");

            var existing = await _trainingPlanService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var updated = await _trainingPlanService.UpdateAsync(plan);
            if (updated == null) return NotFound(); // wenn UpdateAsync intern null zurückgibt

            return NoContent();
        }

        // DELETE: api/TrainingPlan/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _trainingPlanService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // GET /api/TrainingPlan/owner/{ownerId}
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<TrainingPlan>>> GetByOwnerId(int ownerId)
        {
            var plans = await _trainingPlanService.GetByOwnerIdAsync(ownerId);
            if (plans == null || !plans.Any()) return NotFound($"No training plans found for ownerId: {ownerId}");

            return Ok(plans);
        }

        // GET /api/TrainingPlan/{planId}/session/{sessionId}
        [HttpPost("{planId}/session/{sessionId}")]
        public async Task<ActionResult<TrainingPlan>> AddSession(int planId, int sessionId)
        {
            var plan = await _trainingPlanService.AddSessionAsync(planId, sessionId);
            if (plan == null) return NotFound();

            return Ok(plan);
        }

    }
}
