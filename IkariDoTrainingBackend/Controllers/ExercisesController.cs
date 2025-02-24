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
    }
}
