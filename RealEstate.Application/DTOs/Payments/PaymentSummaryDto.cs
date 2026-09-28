namespace RealEstate.Application.DTOs.Payments
{
    public sealed record PaymentSummaryDto
    {
        public Guid ContractId { get; init; }
        public int TotalPayments { get; init; }
        public int PaidCount { get; init; }
        public int PendingCount { get; init; }
        public int OverdueCount { get; init; }
        public int CancelledCount { get; init; }
        public decimal TotalAmount { get; init; }
        public decimal CollectedAmount { get; init; }
        public decimal RemainingAmount { get; init; }
    }
}