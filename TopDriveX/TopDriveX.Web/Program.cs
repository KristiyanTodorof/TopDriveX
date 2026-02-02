using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopDriveX.Application;
using TopDriveX.Domain.Models;
using TopDriveX.Infrastructure.Data;
using TopDriveX.Infrastructure.Data.Initializers;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICES ====================

// Add MVC
builder.Services.AddControllersWithViews();

// Add Infrastructure (DbContext, Identity, UnitOfWork)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application (Services)
builder.Services.AddApplication();

var app = builder.Build();

// ==================== DATABASE INITIALIZATION ====================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        // Get required services
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        logger.LogInformation("🚀 Starting Database Initialization...");
        logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        // Step 1: Migrate database
        logger.LogInformation("📦 Step 1: Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("✅ Database migrations applied successfully");

        // Step 2: Seed Identity (Roles & Users)
        logger.LogInformation("👥 Step 2: Seeding Identity data (Roles & Users)...");
        await IdentitySeeder.SeedAsync(userManager, roleManager, logger);
        logger.LogInformation("✅ Identity data seeded successfully");

        // Step 3: Seed Application Data (Makes, Models, Vehicles, Ads)
        logger.LogInformation("📊 Step 3: Seeding application data...");
        await DatabaseSeeder.SeedDataAsync(context, logger);
        logger.LogInformation("✅ Application data seeded successfully");

        logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        logger.LogInformation("🎉 Database Initialization Completed!");
        logger.LogInformation("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
    catch (Exception ex)
    {
        logger.LogError("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        logger.LogError(ex, "❌ An error occurred while initializing the database");
        logger.LogError("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        throw;
    }
}

// ==================== MIDDLEWARE PIPELINE ====================

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ IMPORTANT: Order matters!
// 1. Authentication (who are you?)
// 2. Authorization (what can you do?)
app.UseAuthentication();
app.UseAuthorization();

// ==================== ROUTES ====================

// Admin area route
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ==================== RUN ====================

app.Run();