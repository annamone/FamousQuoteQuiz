using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Domain.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string Author { get; set; } = null!;

        [InverseProperty(nameof(GameAnswer.Quote))]
        public ICollection<GameAnswer> GameAnswers { get; set; } = new List<GameAnswer>();
    }
}
