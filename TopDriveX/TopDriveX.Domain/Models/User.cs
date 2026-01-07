using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Domain.Models
{
    public class User : BaseEntity
    {
        // Authentication
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Personal Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }

        // Location
        public string? City { get; set; }
        public string? Region { get; set; }
        public string Country { get; set; } = "Bulgaria";

        // Dealer Info
        public string? DealershipName { get; set; }
        public string? DealershipAddress { get; set; }
        public string? Website { get; set; }

        // Profile
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }

        // Status
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }

        // Navigation
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<SavedSearch> SavedSearches { get; set; }
    }
}
