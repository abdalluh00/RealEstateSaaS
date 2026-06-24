using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class WarehousePropertyConfiguration : IEntityTypeConfiguration<WarehouseProperty>
    {
        public void Configure(EntityTypeBuilder<WarehouseProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("WarehouseProperties");

            // ── Properties ────────────────────────────────
            builder.Property(x => x.CeilingHeight)
                   .HasPrecision(10, 2);
            // ✅ reduced precision — ceiling height doesn't need 18 digits

            builder.Property(x => x.LoadingDocks);

            builder.Property(x => x.GateCount);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.ElectricityCapacity)
                   .HasConversion<string>()
                   .HasMaxLength(20);
            // ✅ changed to enum conversion — V220, V380, V440

            // ── Bool Defaults ─────────────────────────────
            builder.Property(x => x.HasOfficeSpace).HasDefaultValue(false);
            builder.Property(x => x.HasSecurityRoom).HasDefaultValue(false);
            builder.Property(x => x.HasCCTV).HasDefaultValue(false);
            builder.Property(x => x.HasFireSystem).HasDefaultValue(false);
            builder.Property(x => x.HasColdStorage).HasDefaultValue(false);
            builder.Property(x => x.HasMosanada).HasDefaultValue(false);
            builder.Property(x => x.IsFenced).HasDefaultValue(false);
            builder.Property(x => x.HasTruckAccess).HasDefaultValue(false);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CeilingHeight);
            builder.HasIndex(x => x.ElectricityCapacity);
            builder.HasIndex(x => x.HasColdStorage);
            // common search filters for warehouse market
        }
    }
}