using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Commands.Quotes;

public record DeleteQuoteCommand(Quote Quote) : IRequest;

public class DeleteQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<DeleteQuoteCommand>
{
    public async Task Handle(DeleteQuoteCommand request, CancellationToken cancellationToken)
        => await quoteRepository.DeleteAsync(request.Quote);
}
