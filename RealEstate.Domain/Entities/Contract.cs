using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Domain.Entities
{
    public class Contract : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public string ContractNumber { get; set; } = string.Empty;
        // Auto-generated: CONT-2024-0001

        // ── Contract Info ─────────────────────────────────
        public ContractType ContractType { get; set; }
        // Rent, Sale

        public ContractStatus ContractStatus { get; set; } = ContractStatus.Active;
        // Active, Expired, Cancelled, Renewed

        // ── Financials ────────────────────────────────────
        public decimal Amount { get; set; }
        public decimal? SecurityDeposit { get; set; }       // مبلغ التأمين

        public PaymentCycle? PaymentCycle { get; set; }
        // Monthly, Quarterly, SemiAnnual, Annual
        // nullable — Sale contracts don't need PaymentCycle

        public PaymentMethod? PaymentMethod { get; set; }
        // Cash, BankTransfer, Cheque, Moyasar
        // nullable — determined per payment not per contract for Rent

        // ── Commission ────────────────────────────────────
        public CommissionType CommissionType { get; set; }
        // Fixed, Percentage

        public decimal Commission { get; set; }

        public CommissionStatus CommissionStatus { get; set; } = CommissionStatus.Pending;
        // Pending, Paid

        // ── Dates ─────────────────────────────────────────
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // nullable — Sale contracts may not have EndDate

        // ── Renewal ───────────────────────────────────────
        public Guid? RenewedFromContractId { get; set; }
        public Contract? RenewedFromContract { get; set; }
        // tracks contract renewal chain

        // ── Cancellation ──────────────────────────────────
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        // points to any property type via base Properties table
        // works for both standalone properties AND units inside buildings
        // (ApartmentProperty with UnitNumber = "101" is still a Property row)

        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public Guid AgentId { get; set; }
        public User Agent { get; set; } = null!;

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        // Payments/Cheques queried by ContractId — no navigation collection needed
    }
}