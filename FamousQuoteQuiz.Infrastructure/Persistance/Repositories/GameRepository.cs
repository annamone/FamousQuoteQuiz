using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Infrastructure.Persistance.Repositories
{
    public class GameRepository(AppDbContext context) : IGameRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IReadOnlyList<GameSession>> GetSessionsAsync(int skip = 0, int take = 50, string? userSearch = null, string? sortBy = null, bool sortDesc = false)
        {
            var games = _context.GameSessions.Include(s => s.User).AsQueryable();
            if (!string.IsNullOrWhiteSpace(userSearch))
            {
                var term = userSearch.Trim().ToLower();
                games = games.Where(s => s.User.UserName.ToLower().Contains(term) || s.User.Email.ToLower().Contains(term));
            }
            games = sortBy?.ToLowerInvariant() switch
            {
                "userid" => sortDesc ? games.OrderByDescending(s => s.UserId) : games.OrderBy(s => s.UserId),
                "startedat" => sortDesc ? games.OrderByDescending(s => s.StartedAt) : games.OrderBy(s => s.StartedAt),
                _ => sortDesc ? games.OrderByDescending(s => s.Id) : games.OrderBy(s => s.Id)
            };
            return await games.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountSessionsAsync(string? userSearch = null)
        {
            var games = _context.GameSessions.Include(s => s.User).AsQueryable();
            if (!string.IsNullOrWhiteSpace(userSearch))
            {
                var term = userSearch.Trim().ToLower();
                games = games.Where(s => s.User.UserName.ToLower().Contains(term) || s.User.Email.ToLower().Contains(term));
            }
            return await games.CountAsync();
        }

        public async Task<IReadOnlyList<GameAnswer>> GetAnswersBySessionIdAsync(int gameSessionId)
        {
            return await _context.GameAnswers
                .Where(a => a.GameSessionId == gameSessionId)
                .OrderBy(a => a.AnsweredAt)
                .ToListAsync();
        }

        public async Task<GameSession?> GetSessionByIdAsync(int id)
        {
            return await _context.GameSessions.FindAsync(id);
        }

        public async Task<GameSession> AddSessionAsync(GameSession session)
        {
            _context.GameSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task AddAnswerAsync(GameAnswer answer)
        {
            _context.GameAnswers.Add(answer);
            await _context.SaveChangesAsync();
        }
    }
}
