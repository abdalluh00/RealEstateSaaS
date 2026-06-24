using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class User : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // ── Role ──────────────────────────────────────────
        public UserRole Role { get; set; }
        // Admin, Agent, Viewer

        // ── Status ────────────────────────────────────────
        public bool IsActive { get; set; } = true;

        // ── Invitation ────────────────────────────────────
        public string? InvitationToken { get; set; }
        public DateTime? InvitationExpiry { get; set; }     // 48 hours from invite
        public bool IsInvitationAccepted { get; set; } = false;
        public DateTime? InvitationAcceptedAt { get; set; } // when they accepted

        // ── Security ──────────────────────────────────────
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}