using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Infrastructure.ExternalServices;
using TopDriveX.Infrastructure.ExternalServices.Nhtsa;

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

            // Add HttpClient for NHTSA Service
            services.AddHttpClient<NhtsaService>();

            // Add NHTSA Services
            services.AddScoped<NhtsaImportService>();


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

            return services;
        }
    }
}
