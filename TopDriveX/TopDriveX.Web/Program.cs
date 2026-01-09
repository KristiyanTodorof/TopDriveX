using TopDriveX.Infrastructure;
using TopDriveX.Infrastructure.Data;
using TopDriveX.Infrastructure.Data.Initializers;
using TopDriveX.Infrastructure.ExternalServices.Nhtsa;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TopDriveX.Infrastructure.Data.Initializers;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Initialize Database
await DatabaseInitializer.InitializeAsync(app.Services);

app.MapGet("/", () => "TopDriveX API");

// Import Makes from NHTSA
app.MapGet("/api/import/makes", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();

        await importService.ImportMakesAsync();

        return Results.Ok("Makes imported successfully!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error: {ex.Message}");
    }
});

// Import Models for all Makes
app.MapGet("/api/import/models", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var makes = await context.Makes.Take(10).ToListAsync(); // First 10 makes

        foreach (var make in makes)
        {
            await importService.ImportModelsForMakeAsync(make.Id);
        }

        return Results.Ok($"Models imported for {makes.Count} makes!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error: {ex.Message}");
    }
});

// Import Vehicle Types
app.MapGet("/api/import/vehicle-types", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();

        await importService.ImportVehicleTypesAsync();

        return Results.Ok("Vehicle types imported successfully!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error: {ex.Message}");
    }
});

app.Run();