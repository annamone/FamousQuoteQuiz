using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using MediatR;

namespace FamousQuoteQuiz.Application.Commands.Quotes;

public record UpdateQuoteCommand(Quote Quote) : IRequest;

public class UpdateQuoteCommandHandler(IQuoteRepository quoteRepository) : IRequestHandler<UpdateQuoteCommand>
{
    public async Task Handle(UpdateQuoteCommand request, CancellationToken cancellationToken)
        => await quoteRepository.UpdateAsync(request.Quote);
}
