using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Contracts.DTO
{
    public class ContractDetailDto
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;

        public ContractType ContractType { get; set; }
        public ContractStatus ContractStatus { get; set; }

        public decimal Amount { get; set; }
        public decimal? SecurityDeposit { get; set; }

        public PaymentCycle? PaymentCycle { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        public CommissionType CommissionType { get; set; }
        public decimal Commission { get; set; }
        public CommissionStatus CommissionStatus { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? CancellationReason { get; set; }
        public string? Notes { get; set; }

        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;

        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;

        public Guid AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
    }
}
