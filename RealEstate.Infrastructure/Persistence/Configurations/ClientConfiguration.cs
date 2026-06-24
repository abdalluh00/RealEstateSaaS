using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

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

            builder.Property(x => x.Notes)
                   .HasMaxLength(2000);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.LeadStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // ✅ removed HasDefaultValue("Lead") — default set on entity level

            builder.Property(x => x.Source)
                   .HasConversion<string>()
                   .HasMaxLength(20);
            // ✅ changed to enum conversion — WhatsApp, Website, Referral, WalkIn, Call

            // ── Relations ─────────────────────────────────
            builder.HasOne(x => x.Company)
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedAgent)
                   .WithMany()
                   .HasForeignKey(x => x.AssignedAgentId)
                   .OnDelete(DeleteBehavior.SetNull);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.LeadStatus);
            builder.HasIndex(x => x.AssignedAgentId);
            builder.HasIndex(x => x.IsActive);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.Phone }).IsUnique();
            // ✅ added IsUnique — no duplicate phone per company
            builder.HasIndex(x => new { x.CompanyId, x.LeadStatus });
            builder.HasIndex(x => new { x.CompanyId, x.AssignedAgentId });
            // agent sees only their assigned clients

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}