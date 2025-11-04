
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;
using TechQuiz.Api.Factory;
using TechQuiz.Api.Dtos;
using BCrypt.Net;
using TechQuiz.Api.Services;
using System.Runtime.InteropServices.Swift;


namespace TechQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly AccesService _acces;
        private readonly QuizzService _quizz;

        public QuizzController(AppDbContext context, AccesService acces, QuizzService quizz)
        {
            _context = context;
            _acces = acces;
            _quizz = quizz;
        }

        // TODO: Implémenter les endpoints pour les quiz
        // Récupérer un quiz, ajouter une partiticipation

        [HttpGet]
        public async Task<IActionResult> GetQuizzesSummary()
        {
            var quizzes = await _context.Quizzes
                .Select(q => new
                {
                    q.Id,
                    q.Name,
                    q.Description,
                    Author = q.User.Name,
                    QuestionCount = q.Questions.Count
                })
                .ToListAsync();

            return Ok(quizzes);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizze(int id)
        {
            var quiz = await _context.Quizzes
                .Where(q => q.Id == id)
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

            return Ok(quiz);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuizz([FromBody] QuizzRequest request)
        {

            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            if (await _quizz.IsExisting(request.Name))
                return BadRequest(new { message = "Nom de quizz invalide ou déjà pris." });


            var quizz = QuizzFactory.CreateQuizz(request, user);
            
            _context.Quizzes.Add(quizz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Quiz créé avec succès." });
        }



        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchQuiz(int id, [FromBody] QuizzPatch request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            if (!string.IsNullOrEmpty(request.Description)) quiz.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Name)) quiz.Name = request.Name;

            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Quiz modifié avec succès." });
        }

        [Authorize]
        [HttpPatch("question/{id}")]
        public async Task<IActionResult> PatchQuestion(int id, [FromBody] QuestionPatch request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            QuizzService.PatchQuestion(request, question);

            return Ok(new { message = "Question modifié avec succès." });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return NotFound();


            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Quizz suprimmé avec succès." });
        }

    }


    public record QuizzPatch(
        string? Name,
        string? Description
    );
    public record QuestionPatch(
        string? Name,
        string? Description,
        int? Point,
        string? Type
    );
    public record ChoicePatch(
        string? Name,
        bool? IsCorrect
    );

}