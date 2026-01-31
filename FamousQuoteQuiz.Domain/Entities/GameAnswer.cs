using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Domain.Entities
{
    public class GameAnswer
    {
        public int Id { get; set; }

        [ForeignKey(nameof(GameSession))]
        public int GameSessionId { get; set; }
        public GameSession GameSession { get; set; }

        [ForeignKey(nameof(Quote))]
        public int QuoteId { get; set; }
        public Quote Quote { get; set; }
        public string SelectedAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.Now;
    }
}
