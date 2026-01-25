using TopDriveX.Infrastructure;
using TopDriveX.Infrastructure.Data;
using TopDriveX.Infrastructure.Data.Initializers;
using TopDriveX.Infrastructure.ExternalServices.Nhtsa;
using TopDriveX.Application;
using TopDriveX.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
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

// API endpoints за import
app.MapGet("/api/import/makes", async (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();
    await importService.ImportMakesAsync();
    return Results.Ok("Makes imported!");
});

app.MapGet("/api/import/models", async (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var makes = await context.Makes.Take(10).ToListAsync();
    foreach (var make in makes)
        await importService.ImportModelsForMakeAsync(make.Id);
    return Results.Ok($"Models imported for {makes.Count} makes!");
});

app.MapGet("/api/import/vehicle-types", async (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();
    await importService.ImportVehicleTypesAsync();
    return Results.Ok("Vehicle types imported!");
});

app.Run();
