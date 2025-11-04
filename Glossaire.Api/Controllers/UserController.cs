using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using TechQuiz.Api.Dtos;
using TechQuiz.Api.Factory;
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

        [Authorize]
        [HttpGet("show/try")]
        public async Task<IActionResult> GetTries()
        {
            // 🔹 Récupère l’utilisateur connecté via le token JWT
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            // 🔹 Récupère toutes ses tentatives avec les infos de quiz
            var tries = await _context.Tries
                .Include(t => t.Quizz)
                .Where(t => t.UserId == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new
                {
                    TryId = t.Id,
                    QuizId = t.QuizzId,
                    QuizName = t.Quizz.Name,
                    Result = t.Result,
                    Date = t.CreatedAt
                })
                .ToListAsync();

            // 🔹 S’il n’a jamais fait de tentative
            if (!tries.Any())
                return Ok(new { message = "Aucune tentative enregistrée pour cet utilisateur." });

            // 🔹 Sinon, on renvoie la liste
            return Ok(new
            {
                message = $"Trouvé {tries.Count} tentative(s).",
                tries
            });
        }


        [Authorize]
        [HttpPost("try")]
        public async Task<IActionResult> AddTry([FromBody] TryRequest request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            var quizz = await _context.Quizzes
                .Include(q => q.Questions) // 🔹 nécessaire pour accéder aux Points
                .FirstOrDefaultAsync(q => q.Id == request.QuizzId);
            if (quizz == null)
                return NotFound(new { message = "Quiz introuvable" });

            int maxPoints = quizz.Questions.Sum(q => q.Point);
            if (request.Result > maxPoints)
            {
                return BadRequest(new
                {
                    message = $"Résultat invalide : {request.Result} > {maxPoints} (score maximum possible)."
                });
            }

            var tryAttempt = QuizzFactory.CreateTry(request, quizz, user);

            _context.Tries.Add(tryAttempt);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tentative ajoutée avec succès." });
        }

    }

    // DTO (Data Transfer Object) pour la requête
    public record UpdateUserRequest(string? Name, string? NewPassword);
}
