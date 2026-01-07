using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;
using TopDriveX.Domain.Enums;

namespace TopDriveX.Domain.Models
{
    public class Advertisement : BaseEntity
    {
        // Foreign Keys
        public Guid VehicleId { get; set; }
        public Guid UserId { get; set; }

        // Details
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; }

        // Status
        public AdvertisementStatus Status { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? FeaturedUntil { get; set; }

        // Dates
        public DateTime PublishedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        // Statistics
        public int ViewCount { get; set; }
        public int ContactCount { get; set; }
        public int FavoriteCount { get; set; }

        // Additional
        public string? VideoUrl { get; set; }
        public string? RejectionReason { get; set; }

        // Navigation
        public virtual Vehicle Vehicle { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
