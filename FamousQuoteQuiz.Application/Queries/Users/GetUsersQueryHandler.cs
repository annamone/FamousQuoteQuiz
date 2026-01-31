using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Users;

public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(
            request.Skip, request.Take, request.Search, request.IsActive,
            request.SortBy, request.SortDesc);
        var total = await userRepository.CountAsync(request.Search, request.IsActive);
        return new GetUsersResult(users, total);
    }
}
