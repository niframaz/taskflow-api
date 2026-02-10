using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskFlow.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            const int maxRetries = 10;
            var delay = TimeSpan.FromSeconds(5);

            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("DbInitializer");

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    logger.LogInformation("Starting database migration attempt {Attempt}", attempt);

                    await context.Database.MigrateAsync();

                    await IdentitySeeder.SeedRolesAsync(serviceProvider);

                    logger.LogInformation("Database migration and seeding completed successfully");

                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Database migration failed on attempt {Attempt}", attempt);

                    if (attempt == maxRetries)
                    {
                        logger.LogCritical("Database migration failed after {MaxRetries} attempts", maxRetries);
                        throw;
                    }

                    await Task.Delay(delay);
                }
            }
        }
    }
}