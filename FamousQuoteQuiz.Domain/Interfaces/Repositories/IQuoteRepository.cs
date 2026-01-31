using FamousQuoteQuiz.Domain.Entities;

namespace FamousQuoteQuiz.Domain.Interfaces.Repositories
{
    public interface IQuoteRepository
    {
        Task<Quote?> GetRandomAsync();
        Task<Quote?> GetByIdAsync(int id);
        Task<IReadOnlyList<int>> GetAllIdsAsync();
        Task<IReadOnlyList<Quote>> GetAllAsync(int skip = 0, int take = 100, string? search = null, string? sortBy = null, bool sortDesc = false);
        Task<int> CountAsync(string? search = null);
        Task<IReadOnlyList<string>> GetRandomAuthorNamesAsync(int count, int excludeQuoteId, string? excludeAuthor = null);
        Task<Quote> AddAsync(Quote quote);
        Task UpdateAsync(Quote quote);
        Task DeleteAsync(Quote quote);
    }
}
