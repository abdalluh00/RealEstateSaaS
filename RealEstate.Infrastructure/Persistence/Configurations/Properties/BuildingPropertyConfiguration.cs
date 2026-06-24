using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class BuildingPropertyConfiguration
    : IEntityTypeConfiguration<BuildingProperty>
    {
        public void Configure(EntityTypeBuilder<BuildingProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("BuildingProperties");

            // ── Properties ────────────────────────────────
            builder.Property(x => x.TotalFloors);
            builder.Property(x => x.UnitsCount);
            builder.Property(x => x.BasementFloors);

            // ── Bool Defaults ─────────────────────────────
            builder.Property(x => x.HasElevator).HasDefaultValue(false);
            builder.Property(x => x.HasParkingFloor).HasDefaultValue(false);
            builder.Property(x => x.HasMosque).HasDefaultValue(false);
            builder.Property(x => x.HasGuard).HasDefaultValue(false);
            builder.Property(x => x.HasGenerator).HasDefaultValue(false);
            builder.Property(x => x.HasCCTV).HasDefaultValue(false);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.TotalFloors);
            builder.HasIndex(x => x.UnitsCount);
        }
    }
}