using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Commands.Quotes;

public record CreateQuoteCommand(Quote Quote) : IRequest<Quote>;

public class CreateQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<CreateQuoteCommand, Quote>
{
    public async Task<Quote> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
        => await quoteRepository.AddAsync(request.Quote);
}
