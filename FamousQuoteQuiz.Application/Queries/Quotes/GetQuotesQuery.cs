using FamousQuoteQuiz.Domain.Entities;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Quotes;

public record GetQuotesQuery(
    int Skip,
    int Take,
    string? Search,
    string SortBy,
    bool SortDesc
) : IRequest<GetQuotesResult>;

public record GetQuotesResult(
    IReadOnlyList<Quote> Quotes,
    int TotalCount
);
