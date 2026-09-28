namespace RealEstate.Application.DTOs.Contracts
{
    public sealed record ContractListDto
    {
        public Guid Id { get; init; }
        public string ContractNumber { get; init; } = string.Empty;
        public string ContractType { get; init; } = string.Empty;
        public string ContractStatus { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string? PaymentCycle { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }

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

        public string CommissionStatus { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}