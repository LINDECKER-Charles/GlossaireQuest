using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechQuiz.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(254)]
        public required string Email { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [Required, MaxLength(255)]
        public required string Password { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; } = "user";

        // Relations
        public ICollection<Quizz> Quizzes { get; set; } = new List<Quizz>();
        public ICollection<Try> Tries { get; set; } = new List<Try>();
    }
}
