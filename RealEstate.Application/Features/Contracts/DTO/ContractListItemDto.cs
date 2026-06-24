using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.DTO
{
    public class ContractListItemDto
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public ContractType ContractType { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;

        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
    }
}
