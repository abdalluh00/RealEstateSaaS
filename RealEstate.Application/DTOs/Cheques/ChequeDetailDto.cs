namespace RealEstate.Application.DTOs.Cheques
{
    public sealed record ChequeDetailDto
    {
        public Guid Id { get; init; }

        // ── Cheque Info ───────────────────────────────────
        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public int ChequeOrder { get; init; }

        // ── Status ────────────────────────────────────────
        public string Status { get; init; } = string.Empty;

        // ── Tracking ──────────────────────────────────────
        public DateTime? DepositedAt { get; init; }
        public DateTime? ClearedAt { get; init; }
        public DateTime? BouncedAt { get; init; }
        public DateTime? CancelledAt { get; init; }

        // ── Bounce ────────────────────────────────────────
        public string? BounceReason { get; init; }
        public Guid? ReplacedByChequeId { get; init; }

        public string? Notes { get; init; }

        // ── Contract ──────────────────────────────────────
        public Guid ContractId { get; init; }
        public string ContractNumber { get; init; } = string.Empty;

        // ── Company ───────────────────────────────────────
        public Guid CompanyId { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}