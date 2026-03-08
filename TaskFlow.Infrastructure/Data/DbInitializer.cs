using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();

            await IdentitySeeder.SeedRolesAsync(services);
        }
    }
}
