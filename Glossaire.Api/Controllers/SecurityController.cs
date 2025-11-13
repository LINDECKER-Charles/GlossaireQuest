using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechQuiz.Api.Data;
using TechQuiz.Api.Factory;
using TechQuiz.Api.Models;
using TechQuiz.Api.Services;
using TechQuiz.Api.Services.Email;

namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public SecurityController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("send-reset-password")]
        public async Task<IActionResult> ResendPassword([FromQuery] string email, [FromServices] EmailSender emailSender)
        {
            string? emailUser = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _context.Users
                .Include(u => u.ResetToken)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });
            
            if (user.ResetToken != null)
            {
                _context.ResetTokens.Remove(user.ResetToken);
                await _context.SaveChangesAsync();
            }
            

            // Crée un token de vérification
            var resetToken = ResetTokenFactory.CreateResetToken(user);
            _context.ResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Envoie l’email de vérification
            string baseUrl = _config["App:BaseUrl"] 
                ?? throw new Exception("App:BaseUrl manquant dans appsettings.json");
            string verifyLink = $"{baseUrl}/reset?token={resetToken.Token}";
            string emailBody = $"Bonjour {user.Name},<br/><br/>Reset votre mot de passe en cliquant sur le lien suivant : <a href=\"{verifyLink}\">Réinitialiser mon mot de passe</a><br/><br/>Merci!";
            
            await emailSender.SendEmailAsync(user.Email, "Réinitialisation de votre mot de passe", emailBody);   
            return Ok(new { message = "Email de réinitialisation envoyé." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string newPassword)
        {
            var resetToken = await _context.ResetTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (resetToken == null || resetToken.IsExpired())
            {
                return BadRequest(new { message = "Token invalide ou expiré." });
            }

            User user = resetToken.User;
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.ResetTokens.Remove(resetToken);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe réinitialisé avec succès." });
        }
    }

}
