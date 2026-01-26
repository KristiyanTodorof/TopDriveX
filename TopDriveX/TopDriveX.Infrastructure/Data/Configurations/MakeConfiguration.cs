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
    public class MakeConfiguration : IEntityTypeConfiguration<Make>
    {
        public void Configure(EntityTypeBuilder<Make> builder)
        {
            builder.ToTable("Makes");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.LogoUrl)
                .HasMaxLength(500);

            builder.Property(m => m.Country)
                .HasMaxLength(100);

            builder.HasIndex(m => m.Name)
                .IsUnique();

            builder.HasMany(m => m.Models)
                .WithOne(model => model.Make)
                .HasForeignKey(model => model.MakeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Vehicles)
                .WithOne(v => v.Make)
                .HasForeignKey(v => v.MakeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
