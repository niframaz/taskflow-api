using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var retries = 10;
            var delay = TimeSpan.FromSeconds(5);

            while (retries > 0)
            {
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    await context.Database.MigrateAsync();

                    await IdentitySeeder.SeedRolesAsync(services);

                    return; // success
                }
                catch (Exception ex)
                {
                    retries--;

                    if (retries == 0)
                        throw;

                    Console.WriteLine($"Database migration failed. Retrying in 5 seconds... {retries} retries left.");
                    await Task.Delay(delay);
                }
            }
        }
    }
}