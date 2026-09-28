namespace RealEstate.Application.DTOs.Appointments
{
    public sealed record AppointmentDetailDto
    {
        public Guid Id { get; init; }
        public DateTime ScheduledAt { get; init; }
        public DateTime? ActualVisitAt { get; init; }
        public int? DurationMinutes { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? Notes { get; init; }
        public string? Feedback { get; init; }
        public string? Result { get; init; }
        public string? CancellationReason { get; init; }
        public DateTime? CancelledAt { get; init; }

        // ── Property ──────────────────────────────────────
        public Guid PropertyId { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string PropertyTitle { get; init; } = string.Empty;
        public string PropertyCity { get; init; } = string.Empty;
        public string PropertyDistrict { get; init; } = string.Empty;
        public string PropertyType { get; init; } = string.Empty;

        // ── Client ────────────────────────────────────────
        public Guid ClientId { get; init; }
        public string ClientName { get; init; } = string.Empty;
        public string ClientPhone { get; init; } = string.Empty;
        public string? ClientEmail { get; init; }

        // ── Agent ─────────────────────────────────────────
        public Guid AgentId { get; init; }
        public string AgentName { get; init; } = string.Empty;
        public string AgentPhone { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}