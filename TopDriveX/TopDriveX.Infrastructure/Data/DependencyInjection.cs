using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TopDriveX.Application.Contracts;
using TopDriveX.Domain.Models;
using TopDriveX.Infrastructure.Repositories;

namespace TopDriveX.Infrastructure.Data
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all Infrastructure services to the service collection
        /// This includes: DbContext, Identity, UnitOfWork, and repositories
        /// </summary>
        /// <param name="services">Service collection to add services to</param>
        /// <param name="configuration">Application configuration (for connection strings, etc.)</param>
        /// <returns>Service collection for method chaining</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ==================== DATABASE CONTEXT ====================

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Get connection string from appsettings.json
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                // Configure SQL Server with retry logic
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Automatically retry failed database operations
                    // Useful for transient errors (network issues, deadlocks, etc.)
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,                    // Retry up to 5 times
                        maxRetryDelay: TimeSpan.FromSeconds(30),  // Max 30 seconds between retries
                        errorNumbersToAdd: null              // Retry all transient errors
                    );

                    // Enable split queries for better performance with includes
                    // sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

#if DEBUG
                // Enable detailed logging in development mode
                // WARNING: This can expose sensitive data (SQL parameters, etc.)
                // NEVER use in production!
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

            // ==================== ASP.NET CORE IDENTITY ====================

            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                // -------------------- PASSWORD REQUIREMENTS --------------------

                options.Password.RequireDigit = true;           // Must contain at least one digit (0-9)
                options.Password.RequireLowercase = true;       // Must contain lowercase letter (a-z)
                options.Password.RequireUppercase = true;       // Must contain uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = false; // Special characters (!@#$%) NOT required
                options.Password.RequiredLength = 6;            // Minimum 6 characters
                options.Password.RequiredUniqueChars = 1;       // At least 1 unique character

                /*
                 * PASSWORD EXAMPLES:
                 * Valid:   "Pass123", "MyPassword1", "Admin123!"
                 * Invalid: "pass123" (no uppercase), "Pass" (too short), "PASSWORD" (no digit)
                 */

                // -------------------- LOCKOUT SETTINGS --------------------

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // Lock for 5 minutes
                options.Lockout.MaxFailedAccessAttempts = 5;                       // Lock after 5 failed attempts
                options.Lockout.AllowedForNewUsers = true;                         // Enable lockout for new users

                /*
                 * LOCKOUT BEHAVIOR:
                 * If user enters wrong password 5 times → Account locked for 5 minutes
                 * After 5 minutes → Lockout counter resets to 0
                 */

                // -------------------- USER SETTINGS --------------------

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Each email can only be used once

                // -------------------- SIGN-IN SETTINGS --------------------

                options.SignIn.RequireConfirmedEmail = false;        // Set to true in production!
                options.SignIn.RequireConfirmedPhoneNumber = false;  // Set to true if using SMS verification
                options.SignIn.RequireConfirmedAccount = false;      // Set to true in production!

                /*
                 * PRODUCTION RECOMMENDATION:
                 * Set RequireConfirmedEmail = true and implement email verification
                 * to prevent fake accounts and ensure valid contact information.
                 */
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()  // Use our DbContext for storage
            .AddDefaultTokenProviders();                       // Add token providers for password reset, etc.

            // ==================== COOKIE AUTHENTICATION ====================

            services.ConfigureApplicationCookie(options =>
            {
                // -------------------- PATHS --------------------

                options.LoginPath = "/Account/Login";              // Redirect here when not authenticated
                options.LogoutPath = "/Account/Logout";            // Logout URL
                options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here when forbidden (403)

                // -------------------- COOKIE SETTINGS --------------------

                options.ExpireTimeSpan = TimeSpan.FromDays(30);    // Cookie expires after 30 days
                options.SlidingExpiration = true;                  // Renew cookie if user is active

                /*
                 * SLIDING EXPIRATION:
                 * If user visits site with 5 days left on cookie → cookie renewed to 30 days
                 * This keeps active users logged in indefinitely
                 */

                options.Cookie.HttpOnly = true;                    // Prevent JavaScript access (XSS protection)
                options.Cookie.SecurePolicy =
                    Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;  // HTTPS only
                options.Cookie.SameSite =
                    Microsoft.AspNetCore.Http.SameSiteMode.Lax;    // CSRF protection
                options.Cookie.Name = "TopDriveX.Auth";            // Cookie name in browser

                // -------------------- SESSION SETTINGS --------------------

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            // ==================== AUTHORIZATION POLICIES (Optional) ====================

            services.AddAuthorization(options =>
            {
                // Policy: Must be Admin
                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireRole("Admin"));

                // Policy: Must be Dealer or Admin
                options.AddPolicy("RequireDealer", policy =>
                    policy.RequireRole("Admin", "Dealer"));

                // Policy: Must be any authenticated user
                options.AddPolicy("RequireAuthenticated", policy =>
                    policy.RequireAuthenticatedUser());

                /*
                 * USAGE IN CONTROLLERS:
                 * [Authorize(Policy = "RequireAdmin")]
                 * public class AdminController : Controller { }
                 */
            });

            // ==================== REPOSITORY PATTERN ====================

            // Register Unit of Work (contains all repositories)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            /*
             * UnitOfWork provides access to all repositories:
             * - Makes, Models, Vehicles, Users, Advertisements, etc.
             * - Centralized SaveChanges() for transaction control
             */

            // ==================== EXTERNAL SERVICES ====================

            // Add any external HTTP services here
            // Example: Email service, SMS service, payment gateway, etc.

            return services;  // Return for method chaining
        }

        /*
         * ==================== USAGE EXAMPLE ====================
         * 
         * In Program.cs:
         * 
         * var builder = WebApplication.CreateBuilder(args);
         * builder.Services.AddInfrastructure(builder.Configuration);
         * 
         * This single line configures:
         * - Database connection
         * - Identity system
         * - Cookie authentication
         * - Authorization policies
         * - Repository pattern
         */
    }
}