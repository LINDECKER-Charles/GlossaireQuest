using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechQuiz.Api.Models
{
    public class Choice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Question))]
        public required int QuestionId { get; set; }
        public required Question Question { get; set; }
    }
}
