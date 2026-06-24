using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class OfficePropertyConfiguration : IEntityTypeConfiguration<OfficeProperty>
    {
        public void Configure(EntityTypeBuilder<OfficeProperty> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("OfficeProperties");

            // ── Properties ────────────────────────────────
            builder.Property(x => x.FloorNumber);
            // ✅ nullable — standalone office may not have floor number

            builder.Property(x => x.Bathrooms);
            // ✅ nullable — small offices may not have separate bathrooms

            builder.Property(x => x.OfficesCount);
            // ✅ nullable — could be open space office

            builder.Property(x => x.MeetingRooms);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.FurnishedStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Bool Defaults ─────────────────────────────
            builder.Property(x => x.HasElevator).HasDefaultValue(false);
            builder.Property(x => x.HasCentralAC).HasDefaultValue(false);
            builder.Property(x => x.HasReceptionArea).HasDefaultValue(false);
            builder.Property(x => x.HasKitchen).HasDefaultValue(false);
            builder.Property(x => x.HasStorage).HasDefaultValue(false);
            builder.Property(x => x.HasCCTV).HasDefaultValue(false);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.FloorNumber);
            builder.HasIndex(x => x.OfficesCount);
            // UnitNumber index already on base PropertyConfiguration ✅
        }
    }
}