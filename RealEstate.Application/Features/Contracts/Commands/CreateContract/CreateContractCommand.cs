using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Contracts.Commands.CreateContract
{
    [Authorize(Roles = "Owner,Admin")]
    public record CreateContractCommand(
        Guid PropertyId,
        Guid ClientId,
        Guid AgentId,
        ContractType ContractType,
        decimal Amount,
        decimal? SecurityDeposit,
        PaymentCycle? PaymentCycle,
        PaymentMethod? PaymentMethod,
        CommissionType CommissionType,
        decimal Commission,
        CommissionStatus CommissionStatus,
        DateTime StartDate,
        DateTime? EndDate,
        string? Notes
    ) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
