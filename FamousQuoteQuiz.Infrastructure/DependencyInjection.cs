using FamousQuoteQuiz.Domain.Interfaces;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Infrastructure.Database;
using FamousQuoteQuiz.Infrastructure.Persistance.Repositories;
using FamousQuoteQuiz.Infrastructure.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamousQuoteQuiz.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            services.AddScoped<IQuoteService, QuoteService>();

            return services;
        }
    }
}
