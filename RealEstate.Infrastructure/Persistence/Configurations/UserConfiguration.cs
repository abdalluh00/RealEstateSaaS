using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            // ── Properties ────────────────────────────────
            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(200);
            // ✅ increased to 200 — Arabic names can be long

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500);

            // ── Invitation ────────────────────────────────
            builder.Property(x => x.InvitationToken)
                   .HasMaxLength(200);

            builder.Property(x => x.IsInvitationAccepted)
                   .HasDefaultValue(false);

            // ── Security ──────────────────────────────────
            builder.Property(x => x.PasswordResetToken)
                   .HasMaxLength(200);

            // ── Flags ─────────────────────────────────────
            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // ── Enum ──────────────────────────────────────
            builder.Property(x => x.Role)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            // ── Relations ─────────────────────────────────
            builder.HasOne(x => x.Company)
                   .WithMany()
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
            // ✅ removed WithMany(c => c.Users) — no nav collection on Company

            // ── Indexes ───────────────────────────────────
            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.Role);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.InvitationToken);
            // quickly find user by invitation token

            builder.HasIndex(x => x.PasswordResetToken);
            // quickly find user by reset token

            // ── Composite Indexes ─────────────────────────
            builder.HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();
            // no duplicate email per company
            builder.HasIndex(x => new { x.CompanyId, x.Role });
            // filter users by role per company
            builder.HasIndex(x => new { x.CompanyId, x.IsActive });
            // active users per company

            // ── Soft Delete ───────────────────────────────
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}