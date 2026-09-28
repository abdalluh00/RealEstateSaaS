using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owners");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Email)
                   .HasMaxLength(200);

            builder.Property(x => x.NationalId)
                   .HasMaxLength(50);

            builder.Property(x => x.Nationality)
                   .HasMaxLength(100);

            builder.Property(x => x.CompanyName)
                   .HasMaxLength(200);

            builder.Property(x => x.IBAN)
                   .HasMaxLength(34);
            // Saudi IBAN is exactly 24 chars but 34 covers all countries

            builder.Property(x => x.CommissionRate)
                   .HasPrecision(5, 2);

            builder.Property(x => x.Notes)
                   .HasMaxLength(2000);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.OwnerType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // ✅ changed to enum — Individual, Company

            // ── Relations ─────────────────────────────────
            builder.HasOne(x => x.Company)
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
            // ✅ removed WithMany(c => c.Owners) — no nav collection on Company

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.OwnerType);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.Phone }).IsUnique();
            // ✅ added IsUnique — no duplicate phone per company
            builder.HasIndex(x => new { x.CompanyId, x.NationalId }).IsUnique();
            builder.HasIndex(x => new { x.CompanyId, x.NationalId })
                  .IsUnique()
                  .HasFilter("[NationalId] IS NOT NULL");
            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}