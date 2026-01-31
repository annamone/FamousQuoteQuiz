using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [InverseProperty(nameof(GameSession.User))]
        public ICollection<GameSession> GameSessions { get; set; } = [];
    }
}
