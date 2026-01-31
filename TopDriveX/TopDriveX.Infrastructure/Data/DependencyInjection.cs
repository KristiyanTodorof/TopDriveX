using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TopDriveX.Application.Contracts;
using TopDriveX.Application.Services;
using TopDriveX.Infrastructure.Repositories;

namespace TopDriveX.Infrastructure.Data
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Infrastructure services with SQL Server
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add DbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Enable automatic retry on transient failures
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                });

                // Enable detailed logging in development
#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

            // Register UnitOfWork and Repository pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Add Infrastructure services with PostgreSQL
        /// </summary>
        public static IServiceCollection AddInfrastructureWithPostgres(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add DbContext with PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    // Enable automatic retry on transient failures
                    npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                });

                // Enable detailed logging in development
#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

            // Register UnitOfWork and Repository pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}