using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
        {
            builder.ToTable("MaintenanceRequests");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.RequestNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Description)
                   .HasMaxLength(2000);

            builder.Property(x => x.ResolutionNotes)
                   .HasMaxLength(2000);

            builder.Property(x => x.ContractorName)
                   .HasMaxLength(200);

            builder.Property(x => x.ContractorPhone)
                   .HasMaxLength(20);

            builder.Property(x => x.Cost)
                   .HasPrecision(18, 2);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.Category)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Plumbing, Electric, AC, Elevator, Structure, Painting, Other

            builder.Property(x => x.Priority)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Low, Medium, High, Urgent

            builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Open, InProgress, Done, Cancelled

            // ── Relations ─────────────────────────────────
            builder.HasOne(x => x.Company)
         .WithMany()
         .HasForeignKey(x => x.CompanyId)
         .OnDelete(DeleteBehavior.Restrict);

            // These have no nav props — add IsRequired()
            builder.HasOne<Property>()
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .IsRequired(false)   // nullable FK
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Client>()
                   .WithMany()
                   .HasForeignKey(x => x.ClientId)
                   .IsRequired(false)   // nullable FK
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.AssignedToId)
                   .IsRequired(false)   // nullable FK
                   .OnDelete(DeleteBehavior.SetNull);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.AssignedToId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.Category);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.Status });
            builder.HasIndex(x => new { x.CompanyId, x.Priority });
            builder.HasIndex(x => new { x.PropertyId, x.Status });
            builder.HasIndex(x => new { x.AssignedToId, x.Status });
            // staff sees their assigned open requests

            builder.HasIndex(x => new { x.CompanyId, x.RequestNumber })
                   .IsUnique();
            // unique request number per company

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}