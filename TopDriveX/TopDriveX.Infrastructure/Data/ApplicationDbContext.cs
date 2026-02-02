using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Models;

namespace TopDriveX.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<
       User,                    // TUser - Our custom User entity
       IdentityRole<Guid>,      // TRole - Identity role with Guid key
       Guid>                    // TKey - Primary key type (Guid instead of string)
    {
        // ==================== CONSTRUCTOR ====================

        /// <summary>
        /// Initializes a new instance of ApplicationDbContext
        /// </summary>
        /// <param name="options">Database context options (connection string, provider, etc.)</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Base constructor configures the context with provided options
        }

        // ==================== DOMAIN DBSETS ====================

        /// <summary>
        /// Makes (car brands) - BMW, Mercedes, Audi, etc.
        /// </summary>
        public DbSet<Make> Makes { get; set; } = null!;

        /// <summary>
        /// Models (car models) - X5, A4, Corolla, etc.
        /// Linked to Makes via MakeId foreign key
        /// </summary>
        public DbSet<Model> Models { get; set; } = null!;

        /// <summary>
        /// Vehicle types - Sedan, SUV, Hatchback, etc.
        /// Optional classification for vehicles
        /// </summary>
        public DbSet<VehicleType> VehicleTypes { get; set; } = null!;

        /// <summary>
        /// Vehicles - the actual cars for sale
        /// Contains all technical details (year, mileage, price, etc.)
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        /// <summary>
        /// Advertisements - published listings for vehicles
        /// One vehicle can have one active advertisement
        /// </summary>
        public DbSet<Advertisement> Advertisements { get; set; } = null!;

        /// <summary>
        /// Vehicle images - photos of the cars
        /// One vehicle can have multiple images
        /// </summary>
        public DbSet<VehicleImage> VehicleImages { get; set; } = null!;

        /// <summary>
        /// User favorites - saved advertisements
        /// Many-to-many relationship between Users and Advertisements
        /// </summary>
        public DbSet<Favorite> Favorites { get; set; } = null!;

        /// <summary>
        /// Saved searches - user's search criteria for notifications
        /// Users can save search filters and get email alerts for new matching ads
        /// </summary>
        public DbSet<SavedSearch> SavedSearches { get; set; } = null!;

        // ==================== IDENTITY DBSETS ====================

        /*
         * Identity DbSets are inherited from IdentityDbContext:
         * - Users (our custom User entity)
         * - Roles (IdentityRole<Guid>)
         * - UserRoles (many-to-many: Users ↔ Roles)
         * - UserClaims (additional user claims)
         * - UserLogins (external login providers like Google, Facebook)
         * - UserTokens (authentication tokens)
         * - RoleClaims (claims attached to roles)
         */

        // ==================== MODEL CONFIGURATION ====================

        /// <summary>
        /// Configures entity mappings, relationships, indexes, and constraints
        /// Called once when the context is first created
        /// </summary>
        /// <param name="modelBuilder">Builder for configuring entities</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Call base method first to configure Identity entities
            base.OnModelCreating(modelBuilder);

            // -------------------- RENAME IDENTITY TABLES --------------------

            // Rename default "AspNet*" tables to cleaner names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

            // -------------------- APPLY ENTITY CONFIGURATIONS --------------------

            // Automatically discover and apply all IEntityTypeConfiguration classes
            // from the same assembly (e.g., MakeConfiguration, ModelConfiguration, etc.)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // -------------------- GLOBAL QUERY FILTERS (SOFT DELETE) --------------------

            // Apply soft delete filter to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if entity inherits from BaseEntity
                if (typeof(Domain.BaseEntities.BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Create expression: e => e.IsDeleted == false
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(Domain.BaseEntities.BaseEntity.IsDeleted));
                    var filter = System.Linq.Expressions.Expression.Lambda(
                        System.Linq.Expressions.Expression.Equal(
                            property,
                            System.Linq.Expressions.Expression.Constant(false)
                        ),
                        parameter
                    );

                    // Apply filter - now all queries will automatically filter out deleted entities
                    entityType.SetQueryFilter(filter);
                }

                // Apply soft delete filter to User entity
                if (entityType.ClrType == typeof(User))
                {
                    // Create expression: u => u.IsDeleted == false
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "u");
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(User.IsDeleted));
                    var filter = System.Linq.Expressions.Expression.Lambda(
                        System.Linq.Expressions.Expression.Equal(
                            property,
                            System.Linq.Expressions.Expression.Constant(false)
                        ),
                        parameter
                    );

                    // Apply filter - deleted users will be hidden from queries
                    entityType.SetQueryFilter(filter);
                }
            }

            /*
             * WHAT SOFT DELETE DOES:
             * 
             * Instead of actually deleting records from the database (DELETE FROM ...),
             * we just mark them as deleted (UPDATE ... SET IsDeleted = 1, DeletedAt = GETUTCDATE()).
             * 
             * The global query filter automatically adds "WHERE IsDeleted = 0" to all queries,
             * so deleted entities are invisible unless you explicitly use IgnoreQueryFilters().
             * 
             * Benefits:
             * - Data preservation (can recover deleted data)
             * - Audit trail (know when something was deleted)
             * - Foreign key integrity (no cascade delete issues)
             */
        }

        // ==================== SAVE CHANGES OVERRIDE ====================

        /// <summary>
        /// Overrides SaveChangesAsync to automatically update timestamps
        /// Called every time changes are saved to the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of state entries written to the database</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get all entities that are being added or modified
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                // Update timestamp for BaseEntity entities
                if (entry.Entity is Domain.BaseEntities.BaseEntity baseEntity)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        baseEntity.UpdatedAt = DateTime.UtcNow;
                    }
                    // CreatedAt is set automatically in BaseEntity constructor
                }

                // Update timestamp for User entities
                if (entry.Entity is User user)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        user.UpdatedAt = DateTime.UtcNow;
                    }
                    // CreatedAt is set automatically in User constructor
                }
            }

            // Call base method to actually save changes
            return base.SaveChangesAsync(cancellationToken);
        }

        /*
         * AUTOMATIC TIMESTAMP UPDATE:
         * 
         * Every time you call SaveChangesAsync(), this method:
         * 1. Finds all entities being modified
         * 2. Sets their UpdatedAt property to current UTC time
         * 3. Then saves to database
         * 
         * Example:
         *   var user = await context.Users.FindAsync(id);
         *   user.FirstName = "New Name";
         *   await context.SaveChangesAsync(); // UpdatedAt automatically set to now
         */
    }
}
