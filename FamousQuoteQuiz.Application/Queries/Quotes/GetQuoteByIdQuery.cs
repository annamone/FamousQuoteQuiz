using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Quotes;

public record GetQuoteByIdQuery(int Id) : IRequest<Quote?>;

public class GetQuoteByIdQueryHandler(IQuoteRepository quoteRepository) : IRequestHandler<GetQuoteByIdQuery, Quote?>
{
    public async Task<Quote?> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
        => await quoteRepository.GetByIdAsync(request.Id);
}
