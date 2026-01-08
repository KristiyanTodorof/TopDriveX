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
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Year)
                .IsRequired();

            builder.Property(v => v.Mileage)
                .IsRequired();

            builder.Property(v => v.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.VIN)
                .HasMaxLength(17);

            builder.Property(v => v.EngineSize)
                .HasColumnType("decimal(4,1)");

            builder.Property(v => v.Color)
                .HasMaxLength(50);

            builder.Property(v => v.InteriorColor)
                .HasMaxLength(50);

            builder.Property(v => v.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Region)
                .HasMaxLength(100);

            builder.Property(v => v.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Bulgaria");

            builder.Property(v => v.Description)
                .HasMaxLength(2000);

            builder.Property(v => v.Features)
                .HasColumnType("nvarchar(max)");

            builder.Property(v => v.FuelType)
                .HasConversion<int>();

            builder.Property(v => v.TransmissionType)
                .HasConversion<int>();

            builder.Property(v => v.BodyStyle)
                .HasConversion<int>();

            builder.Property(v => v.Condition)
                .HasConversion<int>();

            builder.HasIndex(v => v.VIN)
                .IsUnique();

            builder.HasIndex(v => v.Year);
            builder.HasIndex(v => v.Price);
            builder.HasIndex(v => v.Mileage);
            builder.HasIndex(v => new { v.MakeId, v.ModelId });
            builder.HasIndex(v => v.City);

            builder.HasMany(v => v.Images)
                .WithOne(i => i.Vehicle)
                .HasForeignKey(i => i.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Advertisement)
                .WithOne(a => a.Vehicle)
                .HasForeignKey<Advertisement>(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
