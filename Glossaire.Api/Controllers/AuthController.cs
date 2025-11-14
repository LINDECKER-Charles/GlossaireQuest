using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using TechQuiz.Api.Services;

namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized(new { message = "Identifiants invalides" });

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token, role = user.Role });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Vérifie si l’email est déjà pris
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Un compte avec cet email existe déjà." });

            // Crée l’utilisateur
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "USER"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Génére un token directement (auto-login)
            var token = _jwtService.GenerateToken(user);
            return Ok(new { token, role = user.Role });
        }
    }

    public record LoginRequest(string Email, string Password);
    public record RegisterRequest(string Email, string Password, string Name);
}
