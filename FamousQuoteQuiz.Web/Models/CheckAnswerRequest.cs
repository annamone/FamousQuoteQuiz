namespace FamousQuoteQuiz.Web.Models
{
    public class CheckAnswerRequest
    {
        public int QuoteId { get; set; }
        public string? Answer { get; set; }
        public string? DisplayedAuthor { get; set; }
    }
}
