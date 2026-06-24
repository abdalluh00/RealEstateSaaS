using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class ApartmentPropertyConfiguration : IEntityTypeConfiguration<ApartmentProperty>
    {
        public void Configure(EntityTypeBuilder<ApartmentProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("ApartmentProperties");  // ✅ required for TPT

            // ── Properties ────────────────────────────────
            builder.Property(x => x.Bedrooms)
                   .IsRequired();

            builder.Property(x => x.Bathrooms)
                   .IsRequired();

            builder.Property(x => x.FloorNumber);
            // ✅ nullable — not required, standalone apartment may not have floor

            builder.Property(x => x.LivingRooms);

            // ── Bool defaults ─────────────────────────────
            builder.Property(x => x.HasMaidRoom).HasDefaultValue(false);
            builder.Property(x => x.HasElevator).HasDefaultValue(false);
            builder.Property(x => x.HasCentralAC).HasDefaultValue(false);
            builder.Property(x => x.HasBalcony).HasDefaultValue(false);
            builder.Property(x => x.HasStorage).HasDefaultValue(false);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.FurnishedStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.Bedrooms);
            builder.HasIndex(x => x.FloorNumber);
            // UnitNumber index already handled in base PropertyConfiguration
        }
    }
}