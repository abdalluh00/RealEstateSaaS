using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class MaintenanceRequest : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string RequestNumber { get; set; } = string.Empty;
        // Auto-generated: MAINT-2024-0001

        // ── Request Info ──────────────────────────────────
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public MaintenanceCategory Category { get; set; } = MaintenanceCategory.Other;
        // Plumbing, Electric, AC, Elevator, Structure, Painting, Other

        public MaintenancePriority Priority { get; set; } = MaintenancePriority.Medium;
        // Low, Medium, High, Urgent

        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Open;
        // Open, InProgress, Done, Cancelled

        // ── Resolution ────────────────────────────────────
        public decimal? Cost { get; set; }
        public string? ResolutionNotes { get; set; }    // required on close BR-053
        public DateTime? ResolvedAt { get; set; }
        public string? ContractorName { get; set; }     // who did the work
        public string? ContractorPhone { get; set; }

        // ── Relations ─────────────────────────────────────
        // PropertyId — points to any property type via base Properties table
        // works for both standalone AND units inside buildings
        public Guid? PropertyId { get; set; }           // FK only, no navigation

        public Guid? ClientId { get; set; }             // reported by tenant, FK only
        public Guid? AssignedToId { get; set; }         // staff member, FK only

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        // Media queried by MaintenanceRequestId — no navigation collection needed
    }
}