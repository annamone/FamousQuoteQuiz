using FamousQuoteQuiz.Domain.DTOs;
using FamousQuoteQuiz.Domain.Enums;

namespace FamousQuoteQuiz.Domain.Interfaces
{
    public interface IQuoteService
    {
        Task<QuizQuestionDto?> GetQuizQuestionAsync(GameMode mode);
        Task<string?> GetCorrectAuthorAsync(int quoteId);
        Task<bool> CheckAnswerAsync(int quoteId, string answer, string? displayedAuthor = null);
    }
}
