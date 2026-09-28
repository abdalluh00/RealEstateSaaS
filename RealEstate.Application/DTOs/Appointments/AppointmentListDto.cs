namespace RealEstate.Application.DTOs.Appointments
{
    public sealed record AppointmentListDto
    {
        public Guid Id { get; init; }
        public DateTime ScheduledAt { get; init; }
        public string Status { get; init; } = string.Empty;
        public int? DurationMinutes { get; init; }

        // ── Property ──────────────────────────────────────
        public Guid PropertyId { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string PropertyTitle { get; init; } = string.Empty;
        public string PropertyCity { get; init; } = string.Empty;

        // ── Client ────────────────────────────────────────
        public Guid ClientId { get; init; }
        public string ClientName { get; init; } = string.Empty;
        public string ClientPhone { get; init; } = string.Empty;

        // ── Agent ─────────────────────────────────────────
        public Guid AgentId { get; init; }
        public string AgentName { get; init; } = string.Empty;

        public string? Result { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}