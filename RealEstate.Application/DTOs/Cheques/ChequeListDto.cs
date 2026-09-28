namespace RealEstate.Application.DTOs.Cheques
{
    public sealed record ChequeListDto
    {
        public Guid Id { get; init; }

        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public int ChequeOrder { get; init; }

        public string Status { get; init; } = string.Empty;

        public DateTime? DepositedAt { get; init; }
        public DateTime? ClearedAt { get; init; }
        public DateTime? BouncedAt { get; init; }

        // Contract
        public Guid ContractId { get; init; }
        public string ContractNumber { get; init; } = string.Empty;

        public string? BounceReason { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}