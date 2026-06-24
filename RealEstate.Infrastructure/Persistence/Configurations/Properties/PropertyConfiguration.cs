using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations.Properties
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            // ── TPT ───────────────────────────────────────
            builder.ToTable("Properties");  // ✅ required for TPT

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.PropertyCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(x => x.Description)
                   .HasMaxLength(2000);

            builder.Property(x => x.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.District)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Address)
                   .HasMaxLength(500);

            builder.Property(x => x.UnitNumber)
                   .HasMaxLength(50);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Area)
                   .HasColumnType("decimal(10,2)");

            // ── Saudi Legal ───────────────────────────────
            builder.Property(x => x.RegaLicenseNumber)
                   .HasMaxLength(100);

            builder.Property(x => x.DeedNumber)
                   .HasMaxLength(100);

            builder.Property(x => x.MunicipalityNumber)
                   .HasMaxLength(100);

            // ── Enums as string ───────────────────────────
            builder.Property(x => x.Purpose)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.PropertyStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.FacingDirection)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Relations ─────────────────────────────────
            builder.HasOne(x => x.Company)
                   .WithMany()            // ✅ removed WithMany(x => x.Properties) — no nav collection on Company
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Owner)
                   .WithMany()
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Agent)
                   .WithMany()
                   .HasForeignKey(x => x.AgentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.ParentProperty)
                   .WithMany()
                   .HasForeignKey(x => x.ParentPropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.PropertyCode).IsUnique();  // unique code per property
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.PropertyStatus);
            builder.HasIndex(x => x.Purpose);
            builder.HasIndex(x => x.City);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.ParentPropertyId);
            builder.HasIndex(x => x.IsFeatured);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.PropertyStatus });
            builder.HasIndex(x => new { x.CompanyId, x.Purpose });
            builder.HasIndex(x => new { x.CompanyId, x.City });
            builder.HasIndex(x => new { x.ParentPropertyId, x.UnitNumber }).IsUnique();
            // prevents duplicate unit numbers inside same parent

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}