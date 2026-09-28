namespace RealEstate.Application.DTOs.Payments
{
    public sealed record PaymentListDto
    {
        public Guid Id { get; init; }
        public int PaymentNumber { get; init; }
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? PaidDate { get; init; }
        public string PaymentStatus { get; init; } = string.Empty;
        public string? PaymentMethod { get; init; }
        public string? Reference { get; init; }
        public string? Notes { get; init; }
        public Guid ContractId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}