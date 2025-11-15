using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using TechQuiz.Api.Dtos;
using TechQuiz.Api.Factory;
using BCrypt.Net;
using TechQuiz.Api.Services.Email;

namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /* User */
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
                user.Role,
                user.IsVerified
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> PatchUser([FromBody] UpdateUserRequest request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
                user.IsVerified = false;
            }

            if (!string.IsNullOrEmpty(request.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profil mis à jour avec succès." });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Compte utilisateur supprimé avec succès." });
        }

        [Authorize]
        [HttpGet("isVerified")]
        public async Task<IActionResult> IsVerified()
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            return Ok(new
            {
                isVerified = user.IsVerified
            });
        }

        /* Try */
        [Authorize]
        [HttpGet("show/try/{amount?}/{scope?}")]
        public async Task<IActionResult> GetTries(int? amount, int? scope)
        {
            // Récupère l’utilisateur connecté via le token JWT
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            // Récupère toutes ses tentatives avec les infos de quiz
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
                    MaxPoint = t.Quizz.Questions.Sum(q => q.Point),
                    Date = t.CreatedAt,
                    
                })
                .Skip(scope ?? 0)
                .Take(amount ?? 5)
                .ToListAsync();
            // S’il n’a jamais fait de tentative
            if (!tries.Any())
                return Ok(new { message = "Aucune tentative enregistrée pour cet utilisateur." });

            // Sinon, on renvoie la liste
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

        /* Verify */
        [Authorize]
        [HttpPost("send-verify")]
        public async Task<IActionResult> SendVerificationEmail([FromServices] EmailSender emailSender)
        {

            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users
                .Include(u => u.VerificationToken)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });
            
            if (user.IsVerified)
                return BadRequest(new { message = "Votre compte est déjà vérifié." });
            
            if (user.VerificationToken != null)
            {
                _context.VerificationTokens.Remove(user.VerificationToken);
                await _context.SaveChangesAsync();
            }
                
            // Vérifie si l’utilisateur est déjà vérifié
            

            // Crée un token de vérification
            var verifyToken = VerifyTokenFactory.CreateVerifyToken(user);
            _context.VerificationTokens.Add(verifyToken);
            await _context.SaveChangesAsync();

            // Envoie l’email de vérification

            string baseUrl = _config["App:BaseUrl"] 
                ?? throw new Exception("App:BaseUrl manquant dans appsettings.json");
            string verifyLink = $"{baseUrl}/verify?token={verifyToken.Token}";
            string emailBody = $"Bonjour {user.Name},<br/><br/>Veuillez vérifier votre compte en cliquant sur le lien suivant : <a href=\"{verifyLink}\">Vérifier mon compte</a><br/><br/>Merci!";
            
            await emailSender.SendEmailAsync(user.Email, "Vérification de votre compte", emailBody);

            return Ok(new { message = "Email de vérification envoyé." });
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var verifyToken = await _context.VerificationTokens
                .Include(vt => vt.User)
                .FirstOrDefaultAsync(vt => vt.Token == token);

            if (verifyToken == null)
                return BadRequest(new { message = "Token de vérification invalide." });

            if(verifyToken.IsExpired())
                return BadRequest(new { message = "Token de vérification expiré." });
            
            User user = verifyToken.User;
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable pour ce token." });

            user.IsVerified = true;
            _context.VerificationTokens.Remove(verifyToken);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Compte vérifié avec succès." });
        }
    }

    // DTO (Data Transfer Object) pour la requête
    public record UpdateUserRequest(string? Name, string? Password, string? Email);
}
