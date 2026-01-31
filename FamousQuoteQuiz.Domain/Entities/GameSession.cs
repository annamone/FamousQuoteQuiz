using FamousQuoteQuiz.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Domain.Entities
{
    public class GameSession
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public GameMode GameMode { get; set; } = GameMode.Binary;

        public ICollection<GameAnswer> GameAnswers { get; set; } = [];
    }
}
