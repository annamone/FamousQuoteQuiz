using FamousQuoteQuiz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Infrastructure.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Quote> Quotes => Set<Quote>();
        public DbSet<GameSession> GameSessions => Set<GameSession>();
        public DbSet<GameAnswer> GameAnswers => Set<GameAnswer>();

        public static async Task SeedAsync(AppDbContext context)
        {
            const string guestEmail = "guest@email";
            const string adminEmail = "admin@email";

            var guestPasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("admin");

            if (!await context.Users.AnyAsync(u => u.Email == guestEmail))
            {
                context.Users.Add(new User { UserName = "Guest", Email = guestEmail, IsActive = true, PasswordHash = guestPasswordHash });
            }
            else
            {
                var guestUser = await context.Users.FirstOrDefaultAsync(u => u.Email == guestEmail);
                if (guestUser != null && string.IsNullOrEmpty(guestUser.PasswordHash))
                    guestUser.PasswordHash = guestPasswordHash;
            }
            if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
            {
                context.Users.Add(new User { UserName = "admin", Email = adminEmail, IsActive = true, IsAdmin = true, PasswordHash = adminPasswordHash });
            }
            else
            {
                var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
                if (adminUser != null)
                {
                    if (!adminUser.IsAdmin) adminUser.IsAdmin = true;
                    if (string.IsNullOrEmpty(adminUser.PasswordHash)) adminUser.PasswordHash = adminPasswordHash;
                }
            }

            if (!await context.Quotes.AnyAsync())
            {
                context.Quotes.AddRange(
                    new Quote { Text = "The only way to do great work is to love what you do.", Author = "Steve Jobs" },
                    new Quote { Text = "Innovation distinguishes between a leader and a follower.", Author = "Steve Jobs" },
                    new Quote { Text = "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe.", Author = "Albert Einstein" },
                    new Quote { Text = "Imagination is more important than knowledge.", Author = "Albert Einstein" },
                    new Quote { Text = "Be the change that you wish to see in the world.", Author = "Mahatma Gandhi" },
                    new Quote { Text = "In the middle of difficulty lies opportunity.", Author = "Albert Einstein" },
                    new Quote { Text = "The future belongs to those who believe in the beauty of their dreams.", Author = "Eleanor Roosevelt" },
                    new Quote { Text = "It is during our darkest moments that we must focus to see the light.", Author = "Aristotle" }
                );
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
