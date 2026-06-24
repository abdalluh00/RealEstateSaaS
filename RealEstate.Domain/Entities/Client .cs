using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class Client : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NationalId { get; set; }       // رقم الهوية
        public string? Nationality { get; set; }

        // ── Lead Management ───────────────────────────────
        public LeadStatus LeadStatus { get; set; } = LeadStatus.Lead;
        // Lead → Prospect → Active → Inactive

        public LeadSource? Source { get; set; }
        // WhatsApp, Website, Referral, WalkIn, Call

        // ── Flags ─────────────────────────────────────────
        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public Guid? AssignedAgentId { get; set; }
        public User? AssignedAgent { get; set; }
        // Contracts/Appointments queried by ClientId — no navigation collection needed
    }
}