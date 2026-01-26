using TopDriveX.Infrastructure.Data;
using TopDriveX.Infrastructure.Data.Initializers;
using TopDriveX.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

// Add Infrastructure & Application
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Initialize Database and Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

    try
    {
        logger.LogInformation("Starting database migration...");

        // Apply migrations
        await context.Database.MigrateAsync();

        logger.LogInformation("Database migration completed successfully");

        // Seed data
        await DatabaseSeeder.SeedDataAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

// Configure middleware
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();