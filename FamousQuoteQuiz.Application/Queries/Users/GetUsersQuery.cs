using FamousQuoteQuiz.Domain.Entities;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Users;

public record GetUsersQuery(
    int Skip,
    int Take,
    string? Search,
    bool? IsActive,
    string SortBy,
    bool SortDesc
) : IRequest<GetUsersResult>;

public record GetUsersResult(
    IReadOnlyList<User> Users,
    int TotalCount
);
