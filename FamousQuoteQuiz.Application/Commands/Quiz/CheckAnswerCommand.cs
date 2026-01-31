using FamousQuoteQuiz.Domain.Enums;
using MediatR;

namespace FamousQuoteQuiz.Application.Commands.Quiz;

public record CheckAnswerCommand(
    int QuoteId,
    string Answer,
    string? DisplayedAuthor,
    int? GameSessionId,
    int? UserId,
    GameMode GameMode
) : IRequest<CheckAnswerResult>;

public record CheckAnswerResult(bool Correct, string? CorrectAuthor, int? NewGameSessionId);
