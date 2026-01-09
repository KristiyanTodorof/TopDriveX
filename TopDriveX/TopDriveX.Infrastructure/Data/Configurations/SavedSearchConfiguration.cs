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
    public class SavedSearchConfiguration : IEntityTypeConfiguration<SavedSearch>
    {
        public void Configure(EntityTypeBuilder<SavedSearch> builder)
        {
            builder.ToTable("SavedSearches");

            builder.HasKey(ss => ss.Id);

            builder.Property(ss => ss.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ss => ss.SearchCriteria)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(ss => ss.EnableEmailNotifications)
                .HasDefaultValue(true);

            builder.Property(ss => ss.NotificationFrequencyHours)
                .HasDefaultValue(24);

            builder.HasIndex(ss => ss.UserId);
            builder.HasIndex(ss => new { ss.UserId, ss.Name });
        }
    }
}
