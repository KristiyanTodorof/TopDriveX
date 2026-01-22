using TopDriveX.Infrastructure;
using TopDriveX.Infrastructure.Data;
using TopDriveX.Infrastructure.Data.Initializers;
using TopDriveX.Infrastructure.ExternalServices.Nhtsa;
using TopDriveX.Application;
using TopDriveX.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TopDriveX.Infrastructure.Data.Initializers;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure & Application
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Serve static files
app.UseStaticFiles();
app.UseDefaultFiles();

// Initialize Database
await DatabaseInitializer.InitializeAsync(app.Services);

app.MapGet("/", () => Results.Redirect("/index.html"));

// Get all makes
app.MapGet("/api/makes", async (IMakeService makeService) =>
{
    var makes = await makeService.GetAllMakesAsync();
    return Results.Ok(makes);
});

// Get models by make
app.MapGet("/api/makes/{makeId:guid}/models", async (Guid makeId, IModelService modelService) =>
{
    var models = await modelService.GetModelsByMakeIdAsync(makeId);
    return Results.Ok(models);
});

// Get all vehicle types
app.MapGet("/api/vehicle-types", async (IVehicleTypeService vehicleTypeService) =>
{
    var vehicleTypes = await vehicleTypeService.GetAllVehicleTypesAsync();
    return Results.Ok(vehicleTypes);
});

// Import endpoints...
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

app.MapGet("/api/import/models", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<NhtsaImportService>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var makes = await context.Makes.Take(10).ToListAsync();

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