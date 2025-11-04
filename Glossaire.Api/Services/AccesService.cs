using Microsoft.EntityFrameworkCore;
using TechQuiz.Api.Data;
using TechQuiz.Api.Models;


namespace TechQuiz.Api.Services
{
    public class AccesService
    {

        private readonly AppDbContext _context;
        
        public AccesService(AppDbContext context)
        {
            _context = context;
        }

        public bool IsAdmin(User user)
        {
            return user.Role != "ADMIN";
        }

        public async Task<User?> getUser(string? email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}