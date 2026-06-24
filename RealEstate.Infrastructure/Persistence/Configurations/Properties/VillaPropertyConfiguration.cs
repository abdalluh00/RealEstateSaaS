using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class VillaPropertyConfiguration : IEntityTypeConfiguration<VillaProperty>
    {
        public void Configure(EntityTypeBuilder<VillaProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("VillaProperties");

            // ── Properties ────────────────────────────────
            builder.Property(x => x.Bedrooms)
                   .IsRequired();

            builder.Property(x => x.Bathrooms)
                   .IsRequired();

            builder.Property(x => x.Floors);
            // ✅ nullable — villa inside compound may not need this

            builder.Property(x => x.LivingRooms);

            builder.Property(x => x.GardenArea)
                   .HasPrecision(10, 2);
            // ✅ reduced precision — garden area doesn't need 18 digits

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.FurnishedStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Bool Defaults ─────────────────────────────
            builder.Property(x => x.HasMaidRoom).HasDefaultValue(false);
            builder.Property(x => x.HasDriverRoom).HasDefaultValue(false);
            builder.Property(x => x.HasPool).HasDefaultValue(false);
            builder.Property(x => x.HasGarden).HasDefaultValue(false);
            builder.Property(x => x.HasElevator).HasDefaultValue(false);
            builder.Property(x => x.HasMosque).HasDefaultValue(false);
            builder.Property(x => x.HasMajlis).HasDefaultValue(false);
            builder.Property(x => x.HasStorage).HasDefaultValue(false);
            builder.Property(x => x.HasCCTV).HasDefaultValue(false);
            builder.Property(x => x.HasGenerator).HasDefaultValue(false);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.Bedrooms);
            builder.HasIndex(x => x.Floors);
            builder.HasIndex(x => x.HasPool);
            // common search filters in Saudi villa market
        }
    }
}