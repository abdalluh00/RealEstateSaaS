using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.Commands.UpdateContract
{
    [Authorize(Roles = "Owner,Admin")]
    public record UpdateContractCommand : IRequest<ApiResponse<ContractDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }

        public string ContractNumber { get; init; } = string.Empty;
        public ContractType ContractType { get; init; }
        public decimal Amount { get; init; }
        public decimal? SecurityDeposit { get; init; }

        public PaymentCycle? PaymentCycle { get; init; }
        public PaymentMethod? PaymentMethod { get; init; }

        public CommissionType CommissionType { get; init; }
        public decimal Commission { get; init; }
        public ContractStatus ContractStatus { get; init; }
        public CommissionStatus CommissionStatus { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }

        public string? CancellationReason { get; init; }
        public string? Notes { get; init; }

        public Guid PropertyId { get; init; }
        public Guid ClientId { get; init; }
        public Guid AgentId { get; init; }

        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
