using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.PaymentNumber)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.DueDate)
                   .IsRequired();

            builder.Property(x => x.Reference)
                   .HasMaxLength(200);

            builder.Property(x => x.Notes)
                   .HasMaxLength(1000);

            // ── Enums ─────────────────────────────────────
            builder.Property(x => x.PaymentStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // ✅ changed to enum — Pending, Paid, Overdue, Cancelled

            builder.Property(x => x.PaymentMethod)
                   .HasConversion<string>()
                   .HasMaxLength(20);
            // ✅ changed to enum — Cash, BankTransfer, Moyasar, Cheque

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
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.PaymentStatus);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.PaymentMethod);

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.ContractId, x.PaymentStatus });
            builder.HasIndex(x => new { x.CompanyId, x.PaymentStatus });
            builder.HasIndex(x => new { x.CompanyId, x.DueDate });
            // overdue payments dashboard
            builder.HasIndex(x => new { x.ContractId, x.PaymentNumber }).IsUnique();
            // no duplicate payment numbers per contract

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}