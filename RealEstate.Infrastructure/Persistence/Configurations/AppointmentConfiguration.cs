using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.ScheduledAt)
                   .IsRequired();

            builder.Property(x => x.DurationMinutes);

            builder.Property(x => x.Notes)
                   .HasMaxLength(500);

            builder.Property(x => x.Feedback)
                   .HasMaxLength(500);

            builder.Property(x => x.CancellationReason)
                   .HasMaxLength(500);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Result)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // ── Relations ─────────────────────────────────
           
            builder.HasOne(x => x.Property)
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Client)
                   .WithMany()
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Agent)
                   .WithMany()
                   .HasForeignKey(x => x.AgentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Company)
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.ScheduledAt);
            builder.HasIndex(x => x.Status);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.ScheduledAt });
            builder.HasIndex(x => new { x.CompanyId, x.Status });
            builder.HasIndex(x => new { x.AgentId, x.ScheduledAt });
            // agent calendar view

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}