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
    public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
    {
        public void Configure(EntityTypeBuilder<VehicleImage> builder)
        {
            builder.ToTable("VehicleImages");

            builder.HasKey(vi => vi.Id);

            builder.Property(vi => vi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(vi => vi.Caption)
                .HasMaxLength(200);

            builder.Property(vi => vi.IsMain)
                .HasDefaultValue(false);

            builder.Property(vi => vi.DisplayOrder)
                .HasDefaultValue(0);

            builder.HasIndex(vi => vi.VehicleId);
            builder.HasIndex(vi => new { vi.VehicleId, vi.DisplayOrder });
        }
    }
}
