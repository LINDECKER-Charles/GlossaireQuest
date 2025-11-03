
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using BCrypt.Net;
using TechQuiz.Api.Services;


namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzController : ControllerBase
    {

        private readonly AppDbContext _context;

        public QuizzController(AppDbContext context)
        {
            _context = context;
        }

        // TODO: Implémenter les endpoints pour les quiz
        // Récupérer un quiz, ajouter une partiticipation

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuizz([FromBody] QuizzRequest request)
        {
            var quizzName = request.Name;
            if (string.IsNullOrEmpty(quizzName) || await _context.Quizzes.AnyAsync(q => q.Name == quizzName))
            {
                return BadRequest(new { message = "Nom de quizz invalide ou déjà pris." });
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return Unauthorized(new { message = "Utilisateur introuvable ou non authentifié." });

            var quizz = new Quizz
            {
                Name = quizzName,
                Description = request.Description,
                UserId = user.Id,
                Questions = request.Questions.Select(q => new Question
                {
                    Name = q.Name,
                    Description = q.Description,
                    Point = q.Point,
                    Type = q.Type,
                    Choices = q.Choices.Select(c => new Choice
                    {
                        Name = c.Name,
                        IsCorrect = c.IsCorrect
                    }).ToList()
                }).ToList()
            };
            _context.Quizzes.Add(quizz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Quiz créé avec succès." });
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            var quizzes = await _context.Quizzes
                .Select(q => new
                {
                    q.Id,
                    q.Name,
                    q.Description,
                    Author = q.User.Name,
                    Questions = q.Questions.Select(ques => new
                    {
                        ques.Id,
                        ques.Name,
                        ques.Description,
                        ques.Point,
                        ques.Type,
                        Choices = ques.Choices.Select(c => new
                        {
                            c.Id,
                            c.Name,
                            c.IsCorrect
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return Ok(quizzes);
        }
    }


    
    public record QuizzRequest(
        string Name,
        string Description,
        List<QuestionRequest> Questions
    );

    public record QuestionRequest(
        string Name,
        string Description,
        int Point,
        string Type,
        List<ChoiceRequest> Choices
    );

    public record ChoiceRequest(
        string Name,
        bool IsCorrect
    );
}