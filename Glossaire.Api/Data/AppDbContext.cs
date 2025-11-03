using Microsoft.EntityFrameworkCore;
using TechQuiz.Api.Models;

namespace TechQuiz.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quizz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Try> Tries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relations explicites si besoin
            modelBuilder.Entity<Quizz>()
                .HasOne(q => q.User)
                .WithMany(u => u.Quizzes)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quizz)
                .WithMany(z => z.Questions)
                .HasForeignKey(q => q.QuizzId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Choice>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Choices)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Try>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tries)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Try>()
                .HasOne(t => t.Quizz)
                .WithMany(q => q.Tries)
                .HasForeignKey(t => t.QuizzId);
        }
    }
}
