using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class MaintenanceMediaConfiguration : IEntityTypeConfiguration<MaintenanceMedia>
    {
        public void Configure(EntityTypeBuilder<MaintenanceMedia> builder)
        {
            builder.ToTable("MaintenanceMedias");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.FileUrl)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.FileName)
                   .HasMaxLength(200);

            builder.Property(x => x.MimeType)
                   .HasMaxLength(100);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.MediaType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Image, Video

            builder.Property(x => x.UploadedByType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Client, User

            builder.Property(x => x.Stage)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Before, After

            // ── Relations ─────────────────────────────────
            builder.HasOne<MaintenanceRequest>()
                   .WithMany()
                   .HasForeignKey(x => x.MaintenanceRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
            // delete media when request deleted

            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.MaintenanceRequestId);
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.Stage);
            builder.HasIndex(x => x.UploadedByType);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.MaintenanceRequestId, x.Stage });
            // quickly get Before/After photos per request
            builder.HasIndex(x => new { x.MaintenanceRequestId, x.MediaType });
            // filter images vs videos per request

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}