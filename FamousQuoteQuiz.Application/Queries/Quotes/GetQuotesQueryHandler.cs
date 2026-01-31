using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Queries.Quotes;

public class GetQuotesQueryHandler(IQuoteRepository quoteRepository) : IRequestHandler<GetQuotesQuery, GetQuotesResult>
{
    public async Task<GetQuotesResult> Handle(GetQuotesQuery request, CancellationToken cancellationToken)
    {
        var quotes = await quoteRepository.GetAllAsync(
            request.Skip, request.Take, request.Search, request.SortBy, request.SortDesc);
        var total = await quoteRepository.CountAsync(request.Search);
        return new GetQuotesResult(quotes, total);
    }
}
