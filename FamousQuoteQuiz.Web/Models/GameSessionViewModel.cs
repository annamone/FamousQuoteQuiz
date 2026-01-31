namespace FamousQuoteQuiz.Web.Models
{
    public class GameSessionViewModel
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public DateTime StartedAt { get; set; }
        public List<GameAnswerViewModel> Answers { get; set; } = new();
    }

    public class GameAnswerViewModel
    {
        public string QuoteText { get; set; } = "";
        public string CorrectAuthor { get; set; } = "";
        public string SelectedAnswer { get; set; } = "";
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }
    }
}
