using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechQuiz.Api.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public int Point { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = "ONE";

        [ForeignKey(nameof(Quizz))]
        public required int QuizzId { get; set; }
        public required Quizz Quizz { get; set; }

        // Relations
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
    }
}
