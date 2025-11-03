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
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Point { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; }

        [ForeignKey(nameof(Quizz))]
        public int QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        // Relations
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
    }
}
