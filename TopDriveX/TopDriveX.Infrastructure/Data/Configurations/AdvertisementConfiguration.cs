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
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(3000);

            builder.Property(a => a.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.VideoUrl)
                .HasMaxLength(500);

            builder.Property(a => a.RejectionReason)
                .HasMaxLength(500);

            builder.Property(a => a.Status)
                .HasConversion<int>()
                .HasDefaultValue(AdvertisementStatus.Draft);

            builder.Property(a => a.ViewCount)
                .HasDefaultValue(0);

            builder.Property(a => a.ContactCount)
                .HasDefaultValue(0);

            builder.Property(a => a.FavoriteCount)
                .HasDefaultValue(0);

            builder.Property(a => a.IsNegotiable)
                .HasDefaultValue(false);

            builder.Property(a => a.IsFeatured)
                .HasDefaultValue(false);

            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.PublishedAt);
            builder.HasIndex(a => a.ExpiresAt);
            builder.HasIndex(a => a.IsFeatured);
            builder.HasIndex(a => new { a.UserId, a.Status });
            builder.HasIndex(a => a.Price);

            builder.HasMany(a => a.Favorites)
                .WithOne(f => f.Advertisement)
                .HasForeignKey(f => f.AdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
