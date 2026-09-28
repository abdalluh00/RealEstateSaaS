using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class Cheque : BaseEntity
    {
        // ── Cheque Info ───────────────────────────────────
        public string ChequeNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int ChequeOrder { get; set; }
        // 1st, 2nd, 3rd cheque in the contract

        // ── Status ────────────────────────────────────────
        public ChequeStatus Status { get; set; } = ChequeStatus.Pending;
        // Pending, Deposited, Cleared, Bounced, Cancelled

        // ── Tracking Dates ────────────────────────────────
        public DateTime? DepositedAt { get; set; }
        public DateTime? ClearedAt { get; set; }
        public DateTime? BouncedAt { get; set; }    // track when it bounced
        public DateTime? CancelledAt { get; set; }

        // ── Bounce Handling ───────────────────────────────
        public string? BounceReason { get; set; }   // reason for bounce
        public Guid? ReplacedByChequeId { get; set; }
        // if bounced and replaced by new cheque — track the chain

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid ContractId { get; set; }        // FK only
        public Guid CompanyId { get; set; }         // tenant isolation
        public Contract Contract { get; set; } = null!;
    }
}