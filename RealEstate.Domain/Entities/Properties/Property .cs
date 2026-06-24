using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public abstract class Property : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string PropertyCode { get; set; } = string.Empty;
        // Auto-generated: PROP-2024-0001

        // ── Parent (null = standalone, filled = inside building/compound/mall)
        public Guid? ParentPropertyId { get; set; }
        public Property? ParentProperty { get; set; }

        // Unit number only when inside a parent (101, A2, Shop-3)
        public string? UnitNumber { get; set; }

        // ── Core ─────────────────────────────────────────
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public PropertyPurpose Purpose { get; set; }
        // ForRent, ForSale

        public PropertyStatus PropertyStatus { get; set; }
        // Available, Rented, Sold, Reserved

        // ── Pricing ──────────────────────────────────────
        public decimal Price { get; set; }
        public decimal Area { get; set; }

        // ── Location ─────────────────────────────────────
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // ── Common Optional ───────────────────────────────
        public int? ParkingSpots { get; set; }
        public int? AgeInYears { get; set; }
        public FacingDirection? FacingDirection { get; set; }
        // North, South, East, West, NorthEast, NorthWest, SouthEast, SouthWest

        // ── Saudi Legal ───────────────────────────────────
        public string? RegaLicenseNumber { get; set; }  // رقم ترخيص فال
        public string? DeedNumber { get; set; }          // رقم الصك
        public string? MunicipalityNumber { get; set; }  // رقم البلدية

        // ── Flags ─────────────────────────────────────────
        public bool IsFeatured { get; set; } = false;
        public bool IsPublished { get; set; } = false;  // control listing visibility

        // ── Relations ─────────────────────────────────────
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public Guid? OwnerId { get; set; }
        public Owner? Owner { get; set; }

        public Guid? AgentId { get; set; }
        public User? Agent { get; set; }
    }
}