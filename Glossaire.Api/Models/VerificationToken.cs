using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechQuiz.Api.Models
{
    public class VerificationToken
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(32)]
        public required string Token { get; init; }

        [Required]
        public DateTime ExpirationDate { get; init; } = DateTime.UtcNow.AddMinutes(30);

        // Foreign key vers User
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public required User User { get; set; }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationDate;
        }
    }
}
