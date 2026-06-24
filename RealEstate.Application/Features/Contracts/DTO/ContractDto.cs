using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Contracts.DTO
{
    public class ContractDto
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public string PropertyTitle { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public string ContractType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Commission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalPayments { get; set; }
        public int PaidPayments { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Remaining => Amount - TotalPaid;
    }
}
