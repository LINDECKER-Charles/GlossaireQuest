using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechQuiz.Api.Models
{
    public class Try
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Result { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public required User User { get; set; }

        [ForeignKey(nameof(Quizz))]
        public int QuizzId { get; set; }
        public required Quizz Quizz { get; set; }
    }
}
