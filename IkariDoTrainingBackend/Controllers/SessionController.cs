﻿using IkariDoTrainingBackend.Dtos;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Session>>> GetAllSessions()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(sessions);
        }

        // GET api/session/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSessionById(int id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session == null) return NotFound();

            return Ok(session);
        }

        // POST api/session
        [HttpPost]
        public async Task<ActionResult<Session>> CreateSession([FromBody] Session session)
        {
            if (session == null) return BadRequest();

            var created = await _sessionService.CreateAsync(session);
            return CreatedAtAction(nameof(GetSessionById), new { id = created.Id }, created);
        }

        // PUT api/session/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Session>> UpdateSession(int id, [FromBody] Session session)
        {
            if (id != session.Id) return BadRequest("Session ID mismatch");

            var updated = await _sessionService.UpdateAsync(session);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE api/session/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var deleted = await _sessionService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // Exercise einer Session hinzufügen
        [HttpPost("{sessionId}/exercises/{exerciseId}")]
        public async Task<IActionResult> AddExerciseToSession(int sessionId, int exerciseId, [FromQuery] int sets = 1, [FromQuery] int? pauseTime = null)
        {
            var success = await _sessionService.AddExerciseToSessionAsync(sessionId, exerciseId, sets, pauseTime);
            if (!success) return NotFound("Session oder Exercise nicht gefunden.");

            return Ok($"Exercise {exerciseId} wurde zur Session {sessionId} hinzugefügt.");

        }

        // Exercise aus einer Session entfernen
        [HttpDelete("{sessionId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseFromSession(int sessionId, int exerciseId)
        {
            var success = await _sessionService.RemoveExerciseFromSessionAsync(sessionId, exerciseId);
            if (!success) return NotFound("Exercise in der Session nicht gefunden.");

            return Ok($"Exercise {exerciseId} wurde aus Session {sessionId} entfernt.");
        }
    }
}
