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

        //Seed Db
        public static async Task SeedAsync(AppDbContext context)
        {
            const string guestEmail = "guest@email";
            const string adminEmail = "admin@email";

            if (!await context.Users.AnyAsync(u => u.Email == guestEmail))
            {
                var guestPasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
                context.Users.Add(new User { UserName = "Guest", Email = guestEmail, IsActive = true, PasswordHash = guestPasswordHash });
            }

            if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
            {
                var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("admin");
                context.Users.Add(new User { UserName = "admin", Email = adminEmail, IsActive = true, IsAdmin = true, PasswordHash = adminPasswordHash });
            }

            if (!await context.Quotes.AnyAsync())
            {
                context.Quotes.AddRange(
                    new Quote { Text = "Be yourself; everyone else is already taken.", Author = "Oscar Wilde" },
                    new Quote { Text = "It is not our abilities that show what we truly are… it is our choices.", Author = "J. R. R. Tolkien" },
                    new Quote { Text = "Knowing yourself is the beginning of all wisdom.", Author = "Aristotle" },
                    new Quote { Text = "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe.", Author = "Albert Einstein" },
                    new Quote { Text = "Imagination is more important than knowledge.", Author = "Albert Einstein" },
                    new Quote { Text = "Be the change that you wish to see in the world.", Author = "Mahatma Gandhi" },
                    new Quote { Text = "In the middle of difficulty lies opportunity.", Author = "Albert Einstein" },
                    new Quote { Text = "The future belongs to those who believe in the beauty of their dreams.", Author = "Eleanor Roosevelt" },
                    new Quote { Text = "It is during our darkest moments that we must focus to see the light.", Author = "Aristotle" },
                    new Quote { Text = "A room without books is like a body without a soul.", Author = "Marcus Tullius Cicero" },
                    new Quote { Text = "Without music, life would be a mistaken.", Author = "Friedrich Nietzsche" }
                );
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
