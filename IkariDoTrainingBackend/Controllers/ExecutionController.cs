using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly IExecutionService _executionService;

        public ExecutionController(IExecutionService executionService)
        {
            _executionService = executionService;
        }

        // GET: api/<ExecutionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Execution>>> Get()
        {
            var executions = await _executionService.GetAllAsync();
            return Ok(executions);
        }

        // GET api/<ExecutionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Execution>> Get(int id)
        {
            var execution = await _executionService.GetByIdAsync(id);
            if (execution == null)
                return NotFound();

            return Ok(execution);
        }

        // POST api/<ExecutionController>
        [HttpPost]
        public async Task<ActionResult<Execution>> Post([FromBody] Execution execution)
        {
            if(execution == null)
                return BadRequest();

            var created = await _executionService.CreateAsync(execution);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT api/<ExecutionController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Session>> Put(int id, [FromBody] Execution execution)
        {
            if(id != execution.Id) return BadRequest("Execution ID mismatch");

            var updated = await _executionService.UpdateAsync(execution);
            if(updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE api/<ExecutionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _executionService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
