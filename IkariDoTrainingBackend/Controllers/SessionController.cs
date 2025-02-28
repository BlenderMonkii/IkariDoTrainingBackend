using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Dtos.Request;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET api/session
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetAllSessions()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Kein gültiger Benutzer im Token.");

            var userId = int.Parse(userIdString);

            // Nur Sessions, die dem Benutzer gehören (oder isPublic == true, falls du möchtest)
            var sessions = await _sessionService.GetAllByOwnerIdAsync(userId);

            return Ok(sessions);
        }

        // GET api/session/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ResourceOwnerPolicy")]
        public async Task<ActionResult<SessionDto>> GetSessionById(int id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session == null) return NotFound();

            return Ok(session);
        }

        // POST api/session
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SessionDto>> CreateSession([FromBody] SessionDto session)
        {
            if (session == null) return BadRequest();

            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Kein gültiger Benutzer im Token.");

            var userId = int.Parse(userIdString);

            session.OwnerId = userId;

            var created = await _sessionService.CreateAsync(session);
            return CreatedAtAction(nameof(GetSessionById), new { id = created.Id }, created);
        }

        // PUT api/session/5
        [HttpPut("{id}")]
        [Authorize(Policy = "ResourceOwnerPolicy")]
        public async Task<ActionResult<SessionDto>> UpdateSession(int id, [FromBody] SessionDto session)
        {
            if (id != session.Id) return BadRequest("Session ID mismatch");

            var updated = await _sessionService.UpdateAsync(session);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE api/session/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ResourceOwnerPolicy")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var deleted = await _sessionService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // Exercise einer Session hinzufügen
        [HttpPost("{sessionId}/exercise/{exerciseId}")]
        [Authorize] // or [Authorize(Policy = "ResourceOwnerPolicy")] ?
        public async Task<IActionResult> AddExerciseToSession(int sessionId, int exerciseId, [FromBody] AddExerciseToSessionRequest request)
        {
            var success = await _sessionService.AddExerciseToSessionAsync(sessionId, exerciseId, request.Sets, request.PauseTime);

            if (!success) return NotFound("Session oder Exercise nicht gefunden.");

            return Ok($"Exercise {exerciseId} wurde zur Session {sessionId} hinzugefügt.");
        }


        // Exercise aus einer Session entfernen
        [HttpDelete("{sessionId}/exercises/{exerciseId}")]
        [Authorize] // or [Authorize(Policy = "ResourceOwnerPolicy")] ?
        public async Task<IActionResult> RemoveExerciseFromSession(int sessionId, int exerciseId)
        {
            var success = await _sessionService.RemoveExerciseFromSessionAsync(sessionId, exerciseId);
            if (!success) return NotFound("Exercise in der Session nicht gefunden.");

            return Ok($"Exercise {exerciseId} wurde aus Session {sessionId} entfernt.");
        }
    }
}
