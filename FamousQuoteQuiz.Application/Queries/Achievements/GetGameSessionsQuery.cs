using FamousQuoteQuiz.Domain.Enums;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Achievements;

public record GameSessionDto(
    int SessionId,
    int UserId,
    string UserName,
    DateTime StartedAt,
    GameMode GameMode,
    IReadOnlyList<GameAnswerDto> Answers
);

public record GameAnswerDto(
    string QuoteText,
    string CorrectAuthor,
    string SelectedAnswer,
    bool IsCorrect,
    DateTime AnsweredAt
);

public record GetGameSessionsQuery(
    int Skip,
    int Take,
    string? UserSearch,
    string SortBy,
    bool SortDesc
) : IRequest<GetGameSessionsResult>;

public record GetGameSessionsResult(
    IReadOnlyList<GameSessionDto> Sessions,
    int TotalCount
);
