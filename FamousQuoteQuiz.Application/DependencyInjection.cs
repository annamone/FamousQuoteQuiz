using Microsoft.Extensions.DependencyInjection;

namespace FamousQuoteQuiz.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly));

            return services;
        }
    }
}
