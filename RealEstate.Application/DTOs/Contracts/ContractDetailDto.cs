namespace RealEstate.Application.DTOs.Contracts
{
    public sealed record ContractDetailDto
    {
        public Guid Id { get; init; }
        public string ContractNumber { get; init; } = string.Empty;
        public string ContractType { get; init; } = string.Empty;
        public string ContractStatus { get; init; } = string.Empty;

        // ── Financials ────────────────────────────────────
        public decimal Amount { get; init; }
        public decimal? SecurityDeposit { get; init; }
        public string? PaymentCycle { get; init; }
        public string? PaymentMethod { get; init; }

        // ── Commission ────────────────────────────────────
        public string CommissionType { get; init; } = string.Empty;
        public decimal Commission { get; init; }
        public string CommissionStatus { get; init; } = string.Empty;

        // ── Dates ─────────────────────────────────────────
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public DateTime? CancelledAt { get; init; }
        public string? CancellationReason { get; init; }
        public string? Notes { get; init; }

        // ── Renewal ───────────────────────────────────────
        public Guid? RenewedFromContractId { get; init; }
        public string? RenewedFromContractNumber { get; init; }

        // ── Property ──────────────────────────────────────
        public Guid PropertyId { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string PropertyTitle { get; init; } = string.Empty;
        public string PropertyCity { get; init; } = string.Empty;
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