
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
                    QuestionCount = q.Questions.Count,
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

        [HttpGet("summary")]
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

            return Ok(new { message = "Quiz créé avec succès.", id = quizz.Id });
        }

        [Authorize]
        [HttpPost("question")]
        public async Task<IActionResult> AddQuestion(int id, [FromBody] QuestionRequest request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return NotFound(new { message = "Quizz non trouvé." });

            var question = QuizzFactory.CreateQuestion(request, quiz);

            quiz.Questions.Add(question);

            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Question créée avec succès." });
        }

        [Authorize]
        [HttpPost("choice")]
        public async Task<IActionResult> AddChoice(int id, [FromBody] ChoiceRequest request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound(new { message = "Question non trouvée." });

            var choice = QuizzFactory.CreateChoice(request, question);

            question.Choices.Add(choice);
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Choix créé avec succès." });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PatchQuiz([FromQuery] int id, [FromBody] QuizzRequest request)
        {
            Console.WriteLine("PatchQuiz called", id);
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            var newQuiz = QuizzFactory.CreateQuizz(request, user);

            // Forcer l'ancien ID
            newQuiz.Id = id;

            _context.Quizzes.Update(newQuiz);
            await _context.SaveChangesAsync();

            return Ok(newQuiz.Id);
        }

        [Authorize]
        [HttpPatch("question")]
        public async Task<IActionResult> PatchQuestion(int id, [FromBody] QuestionPatch request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });

            var question = await _context.Questions.FindAsync(id);
            
            if (question == null) return NotFound();
                question = QuizzService.PatchQuestion(request, question);

            _context.Questions.Update(question);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Question modifié avec succès." });
        }

        [Authorize]
        [HttpPatch("choice")]
        public async Task<IActionResult> PatchChoice(int id, [FromBody] ChoicePatch request)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });
            
            var choice = await _context.Choices.FindAsync(id);
            if (choice == null) return NotFound();

            choice = QuizzService.PatchChoice(request, choice);

            _context.Choices.Update(choice);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Choix modifié avec succès." });
        }

        [Authorize]
        [HttpDelete]
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

        [Authorize]
        [HttpDelete("question")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound();

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Question supprimée avec succès." });
        }

        [Authorize]
        [HttpDelete("choice")]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;
            User? user = await _acces.getUser(email);
            if (user == null || _acces.IsAdmin(user))
                return Unauthorized(new { message = "Accès non autorisé." });

            var choice = await _context.Choices.FindAsync(id);
            if (choice == null)
                return NotFound();

            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Choix supprimé avec succès." });
        }
    }

}