using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Infrastructure.Persistance.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower());
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(int skip = 0, int take = 50, string? search = null, bool? isActive = null, string? sortBy = null, bool sortDesc = false)
        {
            var users = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
               var term = search.Trim().ToLower();
               users = users.Where(u => u.UserName.ToLower().Contains(term) || u.Email.ToLower().Contains(term));
            }

            if (isActive.HasValue)
                users = users.Where(u => u.IsActive == isActive.Value);

            users = sortBy?.ToLowerInvariant() switch
            {
                "username" => sortDesc ? users.OrderByDescending(u => u.UserName) : users.OrderBy(u => u.UserName),
                "email" => sortDesc ? users.OrderByDescending(u => u.Email) : users.OrderBy(u => u.Email),
                "createdat" => sortDesc ? users.OrderByDescending(u => u.CreatedAt) : users.OrderBy(u => u.CreatedAt),
                _ => sortDesc ? users.OrderByDescending(u => u.Id) : users.OrderBy(u => u.Id)
            };

            return await users.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountAsync(string? search = null, bool? isActive = null)
        {
            var users = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                users = users.Where(u => u.UserName.ToLower().Contains(term) || u.Email.ToLower().Contains(term));
            }
            if (isActive.HasValue)
                users = users.Where(u => u.IsActive == isActive.Value);
            return await users.CountAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
