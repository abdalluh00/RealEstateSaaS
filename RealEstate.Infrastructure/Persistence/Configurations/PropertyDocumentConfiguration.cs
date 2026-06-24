using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PropertyDocumentConfiguration : IEntityTypeConfiguration<PropertyDocument>
    {
        public void Configure(EntityTypeBuilder<PropertyDocument> builder)
        {
            builder.ToTable("PropertyDocuments");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.DocumentName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.FileUrl)
                   .IsRequired()
                   .HasMaxLength(1000);
            // ✅ increased to 1000 — cloud storage URLs can be long

            builder.Property(x => x.DocumentNumber)
                   .HasMaxLength(100);

            builder.Property(x => x.FileName)
                   .HasMaxLength(200);

            builder.Property(x => x.MimeType)
                   .HasMaxLength(100);

            builder.Property(x => x.Notes)
                   .HasMaxLength(2000);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.DocumentType)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            // ── Relations ─────────────────────────────────
            builder.HasOne<Property>()
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
            // ✅ removed nav property — no navigation on PropertyDocument

            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.DocumentType);
            builder.HasIndex(x => x.ExpiryDate);
            // important — track expiring documents (REGA license etc.)

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.PropertyId, x.DocumentType });
            builder.HasIndex(x => new { x.CompanyId, x.ExpiryDate });
            // company-wide expiring documents dashboard

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}