using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class Payment : BaseEntity
    {
        // ── Identity ─────────────────────────────────────
        public int PaymentNumber { get; set; }
        // 1st, 2nd, 3rd installment in the contract

        // ── Financials ────────────────────────────────────
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }

        // ── Status ────────────────────────────────────────
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        // Pending, Paid, Overdue, Cancelled

        // ── Payment Method ────────────────────────────────
        public PaymentMethod? PaymentMethod { get; set; }
        // Cash, BankTransfer, Moyasar, Cheque

        public string? Reference { get; set; }
        // transaction ref, receipt number, or cheque number

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid ContractId { get; set; }
        public Contract Contract { get; set; } = null!;

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}