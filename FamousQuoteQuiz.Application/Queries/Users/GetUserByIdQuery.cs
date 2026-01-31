using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Users;

public record GetUserByIdQuery(int Id) : IRequest<User?>;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => await userRepository.GetByIdAsync(request.Id);
}
