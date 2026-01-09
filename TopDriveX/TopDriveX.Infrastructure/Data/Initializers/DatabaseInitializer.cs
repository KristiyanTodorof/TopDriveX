using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Infrastructure.Data.Initializers
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                logger.LogInformation("Starting database migration...");

                await context.Database.MigrateAsync();

                logger.LogInformation("Database migration completed successfully");

                await SeedDataAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database");
                throw;
            }
        }

        private static async Task SeedDataAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (await context.Makes.AnyAsync())
                {
                    logger.LogInformation("Database already contains data. Skipping seeding.");
                    return;
                }

                logger.LogInformation("Starting data seeding...");

                await context.SaveChangesAsync();
                logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
        public static async Task RecreateAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            logger.LogWarning("Deleting database...");
            await context.Database.EnsureDeletedAsync();

            logger.LogInformation("Creating database...");
            await context.Database.MigrateAsync();

            logger.LogInformation("Database recreated successfully");
        }
    }
}
