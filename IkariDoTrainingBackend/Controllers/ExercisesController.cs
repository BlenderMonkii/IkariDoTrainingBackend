using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Models.Exercises;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/exercises")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // GET: api/exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllExercises(
            [FromQuery] string? type,
            [FromQuery] bool? isPublic,
            [FromQuery] int? ownerId
            )
        {
            var exercises = await _exerciseService.GetAllAsync(type, isPublic, ownerId);
            return Ok(exercises);
        }

        // GET: api/exercises/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseBase>> GetExerciseById(int id)
        {
            var exercise = await _exerciseService.GetByIdAsync(id);
            if (exercise == null)
                return NotFound();

            return Ok(exercise);
        }

        // DELETE: api/exercises/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var success = await _exerciseService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        //// POST: api/exercises/{id}/executions
        //[HttpPost("{id}/executions")]
        //public async Task<ActionResult<Execution>> CreateExecution(int id, Execution execution)
        //{
        //    try
        //    {
        //        var createdExecution = await _executionService.CreateForExerciseAsync(id, execution);
        //        return CreatedAtAction(nameof(GetExecutionById), new { id = createdExecution.Id }, createdExecution);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound($"Exercise with ID {id} not found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //// GET: api/exercises/{exerciseId}/executions/{executionId}
        //[HttpGet("{exerciseId}/executions/{executionId}")]
        //public async Task<ActionResult<Execution>> GetExecutionById(int exerciseId, int executionId)
        //{
        //    var execution = await _executionService.GetByIdAsync(executionId);
        //    if (execution == null)
        //        return NotFound();

        //    if (execution.ExerciseId != exerciseId)
        //        return BadRequest();

        //    return Ok(execution);
        //}

        //// DELETE: api/exercises/{exerciseId}/executions/{executionId}
        //[HttpDelete("{exerciseId}/executions/{executionId}")]
        //public async Task<IActionResult> DeleteExecution(int exerciseId, int executionId)
        //{
        //    var execution = await _executionService.GetByIdAsync(executionId);
        //    if (execution == null)
        //        return NotFound();

        //    if (execution.ExerciseId != exerciseId)
        //        return BadRequest($"Exercise mit der Id {exerciseId} gehört nicht zur Execution");

        //    var success = await _executionService.DeleteAsync(executionId);
        //    if (!success)
        //        return NotFound();

        //    return NoContent();
        //}
    }
}
