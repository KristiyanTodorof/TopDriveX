using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Enums;
using TopDriveX.Domain.Models;

namespace TopDriveX.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // ==================== TABLE MAPPING ====================

            // Map to "Users" table instead of default "AspNetUsers"
            builder.ToTable("Users");

            // Primary key is inherited from IdentityUser<Guid>
            // No need to configure it here

            // ==================== BASIC PROPERTIES ====================

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("User's first name");

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("User's last name");

            // UserType enum stored as integer
            builder.Property(u => u.UserType)
                .HasConversion<int>()
                .HasDefaultValue(UserType.Private)
                .HasComment("User type: 0=Private, 1=Dealer, 2=Admin");

            // ==================== LOCATION PROPERTIES ====================

            builder.Property(u => u.City)
                .HasMaxLength(100)
                .HasComment("City where user is located");

            builder.Property(u => u.Region)
                .HasMaxLength(100)
                .HasComment("Region/Oblast");

            builder.Property(u => u.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Bulgaria")
                .HasComment("Country - defaults to Bulgaria");

            // ==================== DEALER PROPERTIES ====================

            builder.Property(u => u.DealershipName)
                .HasMaxLength(200)
                .HasComment("Name of dealership (for Dealer users only)");

            builder.Property(u => u.DealershipAddress)
                .HasMaxLength(500)
                .HasComment("Physical address of dealership");

            builder.Property(u => u.Website)
                .HasMaxLength(200)
                .HasComment("Dealership website URL");

            // ==================== PROFILE PROPERTIES ====================

            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500)
                .HasComment("URL to user's profile image");

            builder.Property(u => u.Bio)
                .HasMaxLength(1000)
                .HasComment("User biography/description");

            // ==================== STATUS PROPERTIES ====================

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true)
                .HasComment("Whether user account is active");

            builder.Property(u => u.LastLoginAt)
                .HasComment("Last login timestamp");

            // ==================== TIMESTAMP PROPERTIES ====================

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Account creation timestamp");

            builder.Property(u => u.UpdatedAt)
                .HasComment("Last profile update timestamp");

            builder.Property(u => u.DeletedAt)
                .HasComment("Soft delete timestamp");

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("Soft delete flag");

            // ==================== INDEXES ====================

            // These are already created by Identity, but listed here for reference:
            // - IX_Users_NormalizedUserName (UNIQUE)
            // - IX_Users_NormalizedEmail (UNIQUE)

            // Additional custom indexes
            builder.HasIndex(u => u.UserType)
                .HasDatabaseName("IX_Users_UserType");

            builder.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_Users_IsActive");

            builder.HasIndex(u => u.IsDeleted)
                .HasDatabaseName("IX_Users_IsDeleted");

            builder.HasIndex(u => u.City)
                .HasDatabaseName("IX_Users_City");

            builder.HasIndex(u => u.CreatedAt)
                .HasDatabaseName("IX_Users_CreatedAt");

            // ==================== RELATIONSHIPS ====================

            // User -> Advertisements (One-to-Many)
            builder.HasMany(u => u.Advertisements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict) // Don't delete ads when user is deleted
                .HasConstraintName("FK_Users_Advertisements");

            // User -> Favorites (One-to-Many)
            builder.HasMany(u => u.Favorites)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Delete favorites when user is deleted
                .HasConstraintName("FK_Users_Favorites");

            // User -> SavedSearches (One-to-Many)
            builder.HasMany(u => u.SavedSearches)
                .WithOne(ss => ss.User)
                .HasForeignKey(ss => ss.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Delete saved searches when user is deleted
                .HasConstraintName("FK_Users_SavedSearches");

            // ==================== COMPUTED COLUMNS (Optional) ====================

            // Uncomment if you want a computed FullName column in database
            // builder.Property(u => u.FullName)
            //     .HasComputedColumnSql("[FirstName] + ' ' + [LastName]", stored: false);
        }
    }
}
