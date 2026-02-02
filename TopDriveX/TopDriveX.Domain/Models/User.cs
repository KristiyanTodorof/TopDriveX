using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        // ==================== PERSONAL INFO ====================

        /// <summary>
        /// First name of the user (e.g., "Иван")
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user (e.g., "Петров")
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Full name property for convenience
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// User type: Private, Dealer, or Admin
        /// </summary>
        public UserType UserType { get; set; }

        // ==================== LOCATION ====================

        /// <summary>
        /// City where the user is located (e.g., "София", "Пловдив")
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Region/Oblast (e.g., "София-град", "Пловдивска")
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Country - defaults to "Bulgaria"
        /// </summary>
        public string Country { get; set; } = "Bulgaria";

        // ==================== DEALER SPECIFIC INFO ====================

        /// <summary>
        /// Name of the dealership (only for Dealer users)
        /// Example: "Авто Център София"
        /// </summary>
        public string? DealershipName { get; set; }

        /// <summary>
        /// Physical address of the dealership
        /// Example: "ул. Цариградско шосе 100, София"
        /// </summary>
        public string? DealershipAddress { get; set; }

        /// <summary>
        /// Dealership website URL
        /// Example: "https://autocenter.bg"
        /// </summary>
        public string? Website { get; set; }

        // ==================== PROFILE ====================

        /// <summary>
        /// URL to user's profile image
        /// Example: "/images/users/avatar-123.jpg"
        /// </summary>
        public string? ProfileImageUrl { get; set; }

        /// <summary>
        /// User biography/description
        /// Example: "Продавам коли от 10 години. Надежден продавач."
        /// </summary>
        public string? Bio { get; set; }

        // ==================== STATUS & ACTIVITY ====================

        /// <summary>
        /// Whether the user account is active
        /// Inactive users cannot login
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Last time the user logged in
        /// Used for activity tracking
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        // ==================== TIMESTAMPS ====================

        /// <summary>
        /// When the user account was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last time the user profile was updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// When the user was soft-deleted (if deleted)
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Soft delete flag
        /// Deleted users are hidden but not removed from database
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // ==================== NAVIGATION PROPERTIES ====================

        /// <summary>
        /// All advertisements created by this user
        /// </summary>
        public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

        /// <summary>
        /// User's favorite advertisements
        /// </summary>
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        /// <summary>
        /// User's saved searches (for notifications)
        /// </summary>
        public virtual ICollection<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();

        // ==================== INHERITED FROM IdentityUser ====================
        /*
         * The following properties are inherited from IdentityUser<Guid>:
         * 
         * - Id (Guid) - Primary key
         * - UserName (string) - Unique username
         * - NormalizedUserName (string) - Uppercase username for lookups
         * - Email (string) - Email address
         * - NormalizedEmail (string) - Uppercase email for lookups
         * - EmailConfirmed (bool) - Whether email is verified
         * - PasswordHash (string) - Hashed password
         * - SecurityStamp (string) - Random value that changes when credentials change
         * - ConcurrencyStamp (string) - For optimistic concurrency
         * - PhoneNumber (string) - Phone number
         * - PhoneNumberConfirmed (bool) - Whether phone is verified
         * - TwoFactorEnabled (bool) - Whether 2FA is enabled
         * - LockoutEnd (DateTimeOffset?) - When lockout ends
         * - LockoutEnabled (bool) - Whether account can be locked out
         * - AccessFailedCount (int) - Number of failed login attempts
         */
    }
}
