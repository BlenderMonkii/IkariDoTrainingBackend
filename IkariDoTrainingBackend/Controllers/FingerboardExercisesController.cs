using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Models.Exercises;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/exercises/fingerboard")]
    [ApiController]
    public class FingerboardExercisesController : ControllerBase
    {
        private readonly ISpecificExerciseService<FingerboardExercise> _service;

        public FingerboardExercisesController(ISpecificExerciseService<FingerboardExercise> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FingerboardExercise>>> GetAllFingerboardExercises()
        {
            var exercises = await _service.GetAllAsync();
            return Ok(exercises);
        }

        [HttpPost]
        public async Task<ActionResult<FingerboardExercise>> CreateFingerboardExercise(FingerboardExercise exercise)
        {
            var newExercise = await _service.CreateAsync(exercise);
            return CreatedAtAction(nameof(CreateFingerboardExercise), new { id = newExercise.Id }, newExercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFingerboardExercise(int id, FingerboardExercise exercise)
        {
            if (id != exercise.Id) return BadRequest();

            var success = await _service.UpdateAsync(exercise);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
