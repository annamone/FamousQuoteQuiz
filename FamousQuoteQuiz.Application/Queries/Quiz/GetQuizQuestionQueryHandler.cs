using FamousQuoteQuiz.Domain.DTOs;
using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Enums;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Quiz;

public class GetQuizQuestionQueryHandler(
    IQuoteRepository quoteRepository,
    IUserRepository userRepository,
    IGameRepository gameRepository) : IRequestHandler<GetQuizQuestionQuery, GetQuizQuestionResult>
{
    public async Task<GetQuizQuestionResult> Handle(GetQuizQuestionQuery request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
            return new GetQuizQuestionResult(false, "Please log in to play.", null, null, false, 0, 0);

        var allQuoteIds = await quoteRepository.GetAllIdsAsync();
        if (allQuoteIds.Count == 0)
            return new GetQuizQuestionResult(false, "No quotes available. Add quotes in Admin.", null, null, false, 0, 0);

        int? newSessionId = null;
        IReadOnlyList<GameAnswer> sessionAnswers;
        List<int> remainingIds;
        HashSet<int> answeredQuoteIds;

        if (!request.CurrentGameSessionId.HasValue)
        {
            // No session yet - user hasn't answered any questions
            // Don't create a session until they submit their first answer
            sessionAnswers = [];
            remainingIds = allQuoteIds.ToList();
            answeredQuoteIds = [];
        }
        else
        {
            // Session exists - get answers
            sessionAnswers = await gameRepository.GetAnswersBySessionIdAsync(request.CurrentGameSessionId.Value);
            answeredQuoteIds = sessionAnswers.Select(a => a.QuoteId).ToHashSet();
            remainingIds = allQuoteIds.Where(id => !answeredQuoteIds.Contains(id)).ToList();
        }

        var totalQuotes = allQuoteIds.Count;
        var answeredCount = answeredQuoteIds.Count;

        if (remainingIds.Count == 0)
            return new GetQuizQuestionResult(true, "Finished! You answered all quotes.", null, newSessionId, true, totalQuotes, answeredCount);

        var nextQuoteId = remainingIds[Random.Shared.Next(remainingIds.Count)];
        var quote = await quoteRepository.GetByIdAsync(nextQuoteId);
        if (quote == null)
            return new GetQuizQuestionResult(false, "Quote not found.", null, newSessionId, false, totalQuotes, answeredCount);

        QuizQuestionDto question = request.Mode == GameMode.Binary
            ? await BuildBinaryQuestionAsync(quote)
            : await BuildMultipleChoiceQuestionAsync(quote);

        return new GetQuizQuestionResult(true, null, question, newSessionId, false, totalQuotes, answeredCount);
    }

    private async Task<QuizQuestionDto> BuildBinaryQuestionAsync(Quote quote)
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
            var wrongAuthors = await quoteRepository.GetRandomAuthorNamesAsync(1, quote.Id, quote.Author);
            binaryAuthor = wrongAuthors.Count > 0 ? wrongAuthors[0] : "Unknown";
            binaryIsCorrect = false;
        }
        return new QuizQuestionDto(
            quote.Id, quote.Text, quote.Author,
            IsBinaryMode: true, binaryAuthor, binaryIsCorrect, null);
    }

    private async Task<QuizQuestionDto> BuildMultipleChoiceQuestionAsync(Quote quote)
    {
        var wrongs = await quoteRepository.GetRandomAuthorNamesAsync(2, quote.Id, quote.Author);
        var options = new List<string> { quote.Author };
        foreach (var w in wrongs) options.Add(w);
        options = options.OrderBy(_ => Random.Shared.Next()).ToList();
        return new QuizQuestionDto(
            quote.Id, quote.Text, quote.Author,
            IsBinaryMode: false, null, null, options);
    }
}
