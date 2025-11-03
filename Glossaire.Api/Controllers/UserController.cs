using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using BCrypt.Net;

namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Role
            });
        }

        // TODO: Modification d'un utilisateur (nom, mot de passe)
    }

    // DTO (Data Transfer Object) pour la requête
    public record UpdateUserRequest(string? Name, string? NewPassword);
}
