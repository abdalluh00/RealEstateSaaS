using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class LandPropertyConfiguration : IEntityTypeConfiguration<LandProperty>
    {
        public void Configure(EntityTypeBuilder<LandProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("LandProperties");

            // ── Properties ────────────────────────────────
            builder.Property(x => x.StreetWidth)
                   .HasPrecision(10, 2);

            builder.Property(x => x.NumberOfStreets);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.ZoningType)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.LandShape)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Bool Defaults ─────────────────────────────
            builder.Property(x => x.IsCornerLand).HasDefaultValue(false);
            builder.Property(x => x.IsWalled).HasDefaultValue(false);
            builder.Property(x => x.HasElectricity).HasDefaultValue(false);
            builder.Property(x => x.HasWater).HasDefaultValue(false);
            builder.Property(x => x.HasSewer).HasDefaultValue(false);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.ZoningType);
            builder.HasIndex(x => x.IsCornerLand);
            builder.HasIndex(x => x.StreetWidth);
        }
    }
}