using FamousQuoteQuiz.Domain.Entities;

namespace FamousQuoteQuiz.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IReadOnlyList<User>> GetAllAsync(int skip = 0, int take = 50, string? search = null, bool? isActive = null, string? sortBy = null, bool sortDesc = false);
        Task<int> CountAsync(string? search = null, bool? isActive = null);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
