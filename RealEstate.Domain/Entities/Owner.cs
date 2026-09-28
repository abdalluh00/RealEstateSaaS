using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Domain.Entities
{
    public class Owner : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NationalId { get; set; }       // رقم الهوية
        public string? Nationality { get; set; }

        public OwnerType OwnerType { get; set; } = OwnerType.Individual;
        // Individual, Company

        public string? CompanyName { get; set; }
        // filled only when OwnerType = Company

        // ── Financial ────────────────────────────────────
        public string? IBAN { get; set; }
        public decimal? CommissionRate { get; set; }  // % company takes for managing

        // ── Flags ─────────────────────────────────────────
        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        
        public ICollection<Property> Properties { get; set; } = [];
    }
}