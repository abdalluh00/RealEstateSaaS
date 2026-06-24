using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class Company : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;

        // ── Subscription ──────────────────────────────────
        public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Basic;
        // Basic, Pro, Business

        public DateTime SubscriptionExpiry { get; set; }

        // ── Flags ─────────────────────────────────────────
        public bool IsActive { get; set; } = true;

        // No navigation collections — query by CompanyId directly:
        // _context.Users.Where(u => u.CompanyId == id)
        // _context.Properties.Where(p => p.CompanyId == id)
        // _context.Clients.Where(c => c.CompanyId == id)
        // etc.
    }
}