namespace RealEstate.Application.DTOs.Clients
{
    public sealed record ClientDetailDto
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? NationalId { get; init; }
        public string? Nationality { get; init; }
        public string LeadStatus { get; init; } = string.Empty;
        public string? Source { get; init; }
        public bool IsActive { get; init; }
        public string? Notes { get; init; }

        // ── Agent ─────────────────────────────────────────
        public Guid? AssignedAgentId { get; init; }
        public string? AssignedAgentName { get; init; }
        public string? AssignedAgentPhone { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}