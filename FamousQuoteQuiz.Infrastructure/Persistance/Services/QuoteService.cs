using FamousQuoteQuiz.Domain.DTOs;
using FamousQuoteQuiz.Domain.Enums;
using FamousQuoteQuiz.Domain.Interfaces;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;

namespace FamousQuoteQuiz.Infrastructure.Persistance.Services
{
    public class QuoteService(IQuoteRepository quoteRepository) : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository = quoteRepository;

        public async Task<QuizQuestionDto?> GetQuizQuestionAsync(GameMode mode)
        {
            var quote = await _quoteRepository.GetRandomAsync();
            if (quote == null) return null;

            if (mode == GameMode.Binary)
            {
                var showCorrect = Random.Shared.Next(2) == 0;
                string binaryAuthor;
                bool binaryIsCorrect;
                if (showCorrect)
                {
                    binaryAuthor = quote.Author;
                    binaryIsCorrect = true;
                }
                else
                {
                    var wrongAuthors = await _quoteRepository.GetRandomAuthorNamesAsync(1, quote.Id, quote.Author);
                    binaryAuthor = wrongAuthors.Count > 0 ? wrongAuthors[0] : "Unknown";
                    binaryIsCorrect = false;
                }
                return new QuizQuestionDto(
                    quote.Id,
                    quote.Text,
                    quote.Author,
                    IsBinaryMode: true,
                    binaryAuthor,
                    binaryIsCorrect,
                    null
                );
            }

            var wrongs = await _quoteRepository.GetRandomAuthorNamesAsync(2, quote.Id, quote.Author);
            var options = new List<string> { quote.Author };
            foreach (var w in wrongs) options.Add(w);
            options = [.. options.OrderBy(_ => Random.Shared.Next())];

            return new QuizQuestionDto(
                quote.Id,
                quote.Text,
                quote.Author,
                IsBinaryMode: false,
                null,
                null,
                options
            );
        }

        public async Task<string?> GetCorrectAuthorAsync(int quoteId)
        {
            var quote = await _quoteRepository.GetByIdAsync(quoteId);
            return quote?.Author;
        }

        public async Task<bool> CheckAnswerAsync(int quoteId, string answer, string? displayedAuthor = null)
        {
            var quote = await _quoteRepository.GetByIdAsync(quoteId);
            if (quote == null) return false;

            if (displayedAuthor != null)
            {
                var isCorrectAuthor = string.Equals(quote.Author.Trim(), displayedAuthor.Trim(), StringComparison.OrdinalIgnoreCase);
                if (answer.Equals("Yes", StringComparison.OrdinalIgnoreCase)) return isCorrectAuthor;
                if (answer.Equals("No", StringComparison.OrdinalIgnoreCase)) return !isCorrectAuthor;
                return false;
            }

            return string.Equals(quote.Author.Trim(), answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
