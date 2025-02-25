using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace IkariDoTrainingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET api/User/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (user == null) return BadRequest();

            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();

            var existing = await _userService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var updated = await _userService.UpdateAsync(user);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var existing = await _userService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var deleted = await _userService.DeleteAsync(id);
            if (!deleted) return NotFound(); // falls gleichzeitig jemand gelöscht hat

            return Ok(existing);
        }

        // OPTIONALE Erweiterung
        [HttpGet("name/{userName}")]
        public async Task<ActionResult<User>> GetByName(string userName)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
