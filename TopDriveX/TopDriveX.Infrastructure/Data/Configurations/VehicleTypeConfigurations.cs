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
    public class VehicleTypeConfigurations : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.ToTable("VehicleTypes");

            builder.HasKey(vt => vt.Id);

            builder.Property(vt => vt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(vt => vt.Description)
                .HasMaxLength(500);

            builder.Property(vt => vt.NhtsaVehicleTypeId)
                .IsRequired(false);

            builder.HasIndex(vt => vt.Name)
                .IsUnique();

            builder.HasIndex(vt => vt.NhtsaVehicleTypeId)
                .IsUnique();

            builder.HasMany(vt => vt.Vehicles)
                .WithOne(v => v.VehicleType)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
