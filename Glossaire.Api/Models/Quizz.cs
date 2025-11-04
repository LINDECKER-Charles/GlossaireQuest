using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechQuiz.Api.Models
{
    public class Quizz
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        // Foreign key vers User
        [ForeignKey(nameof(User))]
        public required int UserId { get; set; }
        public required User User { get; set; }

        // Relations
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Try> Tries { get; set; } = new List<Try>();
    }
}
