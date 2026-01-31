using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Infrastructure.Persistance.Repositories
{
    public class QuoteRepository(AppDbContext context) : IQuoteRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Quote?> GetRandomAsync()
        {
            var count = await _context.Quotes.CountAsync();
            if (count == 0) return null;
            var skip = Random.Shared.Next(0, count);
            return await _context.Quotes.OrderBy(q => q.Id).Skip(skip).FirstOrDefaultAsync();
        }

        public async Task<Quote?> GetByIdAsync(int id)
        {
            return await _context.Quotes.FindAsync(id);
        }

        public async Task<IReadOnlyList<int>> GetAllIdsAsync()
        {
            return await _context.Quotes.Select(q => q.Id).ToListAsync();
        }

        public async Task<IReadOnlyList<Quote>> GetAllAsync(int skip = 0, int take = 100, string? search = null, string? sortBy = null, bool sortDesc = false)
        {
            var quotes = _context.Quotes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                quotes = quotes.Where(x => x.Text.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                    || x.Author.Contains(term, StringComparison.CurrentCultureIgnoreCase));
            }

            quotes = sortBy?.ToLowerInvariant() switch
            {
                "author" => sortDesc ? quotes.OrderByDescending(x => x.Author) : quotes.OrderBy(x => x.Author),
                "text" => sortDesc ? quotes.OrderByDescending(x => x.Text) : quotes.OrderBy(x => x.Text),
                _ => sortDesc ? quotes.OrderByDescending(x => x.Id) : quotes.OrderBy(x => x.Id)
            };
            return await quotes.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountAsync(string? search = null)
        {
            var quotes = _context.Quotes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                quotes = quotes.Where(x => x.Text.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Author.Contains(term, StringComparison.CurrentCultureIgnoreCase));
            }
            return await quotes.CountAsync();
        }

        public async Task<Quote> AddAsync(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
        }

        public async Task UpdateAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Quote quote)
        {
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<string>> GetRandomAuthorNamesAsync(int count, int excludeQuoteId, string? excludeAuthor = null)
        {
            var query = _context.Quotes.Where(q => q.Id != excludeQuoteId);
            if (!string.IsNullOrWhiteSpace(excludeAuthor))
                query = query.Where(q => q.Author != excludeAuthor);

            var authors = await query.Select(q => q.Author).Distinct().ToListAsync();
            if (authors.Count < count) return authors;
            return authors.OrderBy(_ => Random.Shared.Next()).Take(count).ToList();
        }
    }
}
