namespace RealEstate.Application.DTOs.Maintenance
{
    public sealed record MaintenanceDetailDto
    {
        public Guid Id { get; init; }
        public string RequestNumber { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Category { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;

        // ── Resolution ────────────────────────────────────
        public decimal? Cost { get; init; }
        public string? ResolutionNotes { get; init; }
        public DateTime? ResolvedAt { get; init; }
        public string? ContractorName { get; init; }
        public string? ContractorPhone { get; init; }

        // ── Relations ─────────────────────────────────────
        public Guid? PropertyId { get; init; }
        public string? PropertyTitle { get; init; }
        public string? PropertyCode { get; init; }
        public Guid? ClientId { get; init; }
        public string? ClientName { get; init; }
        public string? ClientPhone { get; init; }
        public Guid? AssignedToId { get; init; }
        public string? AssignedToName { get; init; }
        public string? AssignedToPhone { get; init; }

        // ── Media ─────────────────────────────────────────
        public IReadOnlyList<MaintenanceMediaDto> Media { get; init; } = [];

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}