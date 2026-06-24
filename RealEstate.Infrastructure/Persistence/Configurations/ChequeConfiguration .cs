using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class ChequeConfiguration : IEntityTypeConfiguration<Cheque>
    {
        public void Configure(EntityTypeBuilder<Cheque> builder)
        {
            builder.ToTable("Cheques");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.ChequeNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.BankName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.DueDate)
                   .IsRequired();

            builder.Property(x => x.ChequeOrder)
                   .IsRequired();

            builder.Property(x => x.BounceReason)
                   .HasMaxLength(500);

            builder.Property(x => x.Notes)
                   .HasMaxLength(1000);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // Pending, Deposited, Cleared, Bounced, Cancelled

            // ── Self Reference (replaced cheque) ──────────
            builder.HasOne<Cheque>()
                   .WithMany()
                   .HasForeignKey(x => x.ReplacedByChequeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Relations ─────────────────────────────────
            builder.HasOne<Contract>()
                   .WithMany()
                   .HasForeignKey(x => x.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.ReplacedByChequeId);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.ContractId, x.ChequeOrder }).IsUnique();
            // no duplicate cheque order per contract
            builder.HasIndex(x => new { x.CompanyId, x.Status });
            builder.HasIndex(x => new { x.CompanyId, x.DueDate });
            // upcoming cheques dashboard

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}