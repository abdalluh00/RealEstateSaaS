using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts");

            builder.HasKey(x => x.Id);

            // ── Identity ──────────────────────────────────
            builder.Property(x => x.ContractNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            // ── Financials ────────────────────────────────
            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.SecurityDeposit)
                   .HasPrecision(18, 2);

            builder.Property(x => x.Commission)
                   .HasPrecision(18, 2)
                   .IsRequired();

            // ── Notes ─────────────────────────────────────
            builder.Property(x => x.CancellationReason)
                   .HasMaxLength(1000);

            builder.Property(x => x.Notes)
                   .HasMaxLength(2000);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.ContractType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.ContractStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.PaymentCycle)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.PaymentMethod)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.CommissionType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.CommissionStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            // ── Renewal ───────────────────────────────────
            builder.HasOne(x => x.RenewedFromContract)
                   .WithMany()
                   .HasForeignKey(x => x.RenewedFromContractId)
                   .OnDelete(DeleteBehavior.Restrict);

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
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                   .WithMany()           // ✅ removed WithMany(x => x.Contracts) — no nav collection on Company
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.ContractNumber })
                   .IsUnique();

            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.ContractStatus);
            builder.HasIndex(x => x.EndDate);          // for expiry checks
            builder.HasIndex(x => x.RenewedFromContractId);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.ContractStatus });
            builder.HasIndex(x => new { x.PropertyId, x.ContractStatus });
            builder.HasIndex(x => new { x.ClientId, x.ContractStatus });
            builder.HasIndex(x => new { x.CompanyId, x.EndDate });
            // useful for expiring contracts dashboard
        }
    }
}