using FamousQuoteQuiz.Domain.DTOs;
using FamousQuoteQuiz.Domain.Enums;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Quiz;

public record GetQuizQuestionQuery(
    GameMode Mode,
    int? CurrentGameSessionId,
    int? UserId
) : IRequest<GetQuizQuestionResult>;

public record GetQuizQuestionResult(
    bool Success,
    string? Message,
    QuizQuestionDto? Question,
    int? NewGameSessionId,
    bool IsFinished,
    int TotalQuotes,
    int AnsweredCount
);
