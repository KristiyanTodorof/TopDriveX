using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Models;

namespace TopDriveX.Infrastructure.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Notes)
                .HasMaxLength(500);

            builder.HasIndex(f => new { f.UserId, f.AdvertisementId })
                .IsUnique();

            builder.HasIndex(f => f.UserId);
            builder.HasIndex(f => f.AdvertisementId);
        }
    }
}
