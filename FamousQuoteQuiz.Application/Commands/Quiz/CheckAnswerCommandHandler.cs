using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Commands.Quiz;

public class CheckAnswerCommandHandler(
    IQuoteRepository quoteRepository,
    IGameRepository gameRepository) : IRequestHandler<CheckAnswerCommand, CheckAnswerResult>
{
    public async Task<CheckAnswerResult> Handle(CheckAnswerCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.QuoteId);
        if (quote == null)
            return new CheckAnswerResult(false, null, null);

        var correct = IsAnswerCorrect(quote.Author, request.Answer, request.DisplayedAuthor);
        var correctAuthor = quote.Author;

        int? newSessionId = null;
        int sessionId;

        // Create session on first answer if it doesn't exist yet
        if (!request.GameSessionId.HasValue && request.UserId.HasValue)
        {
            var session = await gameRepository.AddSessionAsync(new GameSession 
            { 
                UserId = request.UserId.Value,
                GameMode = request.GameMode
            });
            newSessionId = session.Id;
            sessionId = session.Id;
        }
        else if (request.GameSessionId.HasValue)
        {
            sessionId = request.GameSessionId.Value;
        }
        else
        {
            return new CheckAnswerResult(correct, correctAuthor, null);
        }

        await gameRepository.AddAnswerAsync(new GameAnswer
        {
            GameSessionId = sessionId,
            QuoteId = request.QuoteId,
            SelectedAnswer = request.Answer ?? "",
            IsCorrect = correct
        });

        return new CheckAnswerResult(correct, correctAuthor, newSessionId);
    }

    private static bool IsAnswerCorrect(string quoteAuthor, string answer, string? displayedAuthor)
    {
        if (displayedAuthor != null)
        {
            var isCorrectAuthor = string.Equals(quoteAuthor.Trim(), displayedAuthor.Trim(), StringComparison.OrdinalIgnoreCase);
            if (answer.Equals("Yes", StringComparison.OrdinalIgnoreCase)) return isCorrectAuthor;
            if (answer.Equals("No", StringComparison.OrdinalIgnoreCase)) return !isCorrectAuthor;
            return false;
        }
        return string.Equals(quoteAuthor.Trim(), answer.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
