using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
    {
        public void Configure(EntityTypeBuilder<PropertyMedia> builder)
        {
            builder.ToTable("PropertyMedias");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.MediaUrl)
                   .IsRequired()
                   .HasMaxLength(1000);
            // ✅ increased to 1000 — cloud storage URLs can be long

            builder.Property(x => x.Title)
                   .HasMaxLength(200);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.FileName)
                   .HasMaxLength(200);

            builder.Property(x => x.MimeType)
                   .HasMaxLength(100);

            builder.Property(x => x.IsCover)
                   .HasDefaultValue(false);

            builder.Property(x => x.SortOrder)
                   .HasDefaultValue(0);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.MediaType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // ✅ removed HasDefaultValue(MediaType.Image) — default set on entity level

            // ── Relations ─────────────────────────────────
            builder.HasOne<Property>()
         .WithMany(x => x.Media)        // ← use nav on Property entity
         .HasForeignKey(x => x.PropertyId)
         .IsRequired()
         .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.IsCover);
            builder.HasIndex(x => x.MediaType);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.PropertyId, x.SortOrder });
            builder.HasIndex(x => new { x.PropertyId, x.IsCover });
            // quickly find cover image per property

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}