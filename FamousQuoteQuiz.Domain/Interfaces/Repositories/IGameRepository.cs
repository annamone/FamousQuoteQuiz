using FamousQuoteQuiz.Domain.Entities;

namespace FamousQuoteQuiz.Domain.Interfaces.Repositories
{
    public interface IGameRepository
    {
        Task<IReadOnlyList<GameSession>> GetSessionsAsync(int skip = 0, int take = 50, string? userSearch = null, string? sortBy = null, bool sortDesc = false);
        Task<int> CountSessionsAsync(string? userSearch = null);
        Task<IReadOnlyList<GameAnswer>> GetAnswersBySessionIdAsync(int gameSessionId);
        Task<GameSession?> GetSessionByIdAsync(int id);
        Task<GameSession> AddSessionAsync(GameSession session);
        Task AddAnswerAsync(GameAnswer answer);
    }
}
