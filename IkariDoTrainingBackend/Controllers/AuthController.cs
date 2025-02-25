using Microsoft.AspNetCore.Mvc;
using IkariDoTrainingBackend.Models;
using BCrypt.Net; // Installiere das NuGet-Paket "BCrypt.Net-Next" oder ähnlich
using System.Linq;
using IkariDoTrainingBackend.Data;
using Microsoft.AspNetCore.Identity.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly ApplicationDbContext _context;

    public AuthController(JwtTokenService jwtTokenService, ApplicationDbContext context)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // 1. Gültigkeit der Anfrage prüfen
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Email und Passwort sind erforderlich");
        }

        // 2. Nutzer anhand der Email aus der DB laden
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user == null)
        {
            // User existiert nicht
            return Unauthorized("Ungültige Anmeldedaten");
        }

        // 3. Passwort-Hash verifizieren
        //    Hier wird das Eingegebene 'request.Password' gehasht und mit dem gespeicherten Hash verglichen
        //    BCrypt.Net.BCrypt.Verify macht das automatisch.
        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            // Falsches Passwort
            return Unauthorized("Ungültige Anmeldedaten");
        }

        // 4. Token generieren und zurückgeben
        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}
