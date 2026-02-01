using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Infrastructure.Database;
using FamousQuoteQuiz.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamousQuoteQuiz.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            //DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            //Repositories
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
