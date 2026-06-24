using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Logo)
                   .HasMaxLength(500);

            builder.Property(x => x.Address)
                   .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.SubscriptionExpiry)
                   .IsRequired();

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.SubscriptionPlan)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            // ✅ changed to enum — Basic, Pro, Business

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.SubscriptionPlan);
            builder.HasIndex(x => x.SubscriptionExpiry);
            // useful for subscription expiry checks

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}