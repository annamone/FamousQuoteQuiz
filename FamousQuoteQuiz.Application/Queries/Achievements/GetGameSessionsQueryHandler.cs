using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Achievements;

public class GetGameSessionsQueryHandler(
    IGameRepository gameRepository,
    IUserRepository userRepository,
    IQuoteRepository quoteRepository) : IRequestHandler<GetGameSessionsQuery, GetGameSessionsResult>
{
    public async Task<GetGameSessionsResult> Handle(GetGameSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await gameRepository.GetSessionsAsync(
            request.Skip, request.Take, request.UserSearch, request.SortBy, request.SortDesc);
        var total = await gameRepository.CountSessionsAsync(request.UserSearch);

        var sessionDtos = new List<GameSessionDto>();
        foreach (var s in sessions)
        {
            var user = await userRepository.GetByIdAsync(s.UserId);
            var answers = await gameRepository.GetAnswersBySessionIdAsync(s.Id);
            var answerDtos = new List<GameAnswerDto>();
            foreach (var a in answers)
            {
                var quote = await quoteRepository.GetByIdAsync(a.QuoteId);
                answerDtos.Add(new GameAnswerDto(
                    quote?.Text ?? "-",
                    quote?.Author ?? "-",
                    a.SelectedAnswer,
                    a.IsCorrect,
                    a.AnsweredAt
                ));
            }
            sessionDtos.Add(new GameSessionDto(
                s.Id,
                s.UserId,
                user?.UserName ?? "Unknown",
                s.StartedAt,
                s.GameMode,
                answerDtos
            ));
        }

        return new GetGameSessionsResult(sessionDtos, total);
    }
}
