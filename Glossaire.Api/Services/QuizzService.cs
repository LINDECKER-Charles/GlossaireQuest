using Microsoft.EntityFrameworkCore;
using TechQuiz.Api.Controllers;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;


namespace TechQuiz.Api.Services
{
    public class QuizzService
    {

        private readonly AppDbContext _context;

        public QuizzService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExisting(string name)
        {
            return string.IsNullOrEmpty(name) || await _context.Quizzes.AnyAsync(q => q.Name == name);
        }

        public static Question PatchQuestion(QuestionPatch request, Question q)
        {
            if (!string.IsNullOrEmpty(request.Description)) q.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Name)) q.Name = request.Name;
            if (request.Point != null && IsNotNegative((int)request.Point)) q.Point = (int)request.Point;
            if (!string.IsNullOrEmpty(request.Type) && IsValideType(request.Type)) q.Type = request.Type;
            return q;
        }

        private static bool IsNotNegative(int num)
        {
            return num >= 0;
        }
        
        private static bool IsValideType(string type)
        {
            return type == "ONE" || type == "MULTI";
        }
    }
}