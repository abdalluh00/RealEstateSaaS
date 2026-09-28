using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Queries.GetPagedContracts
{
    public sealed class GetPagedContractsQuery
        : IRequest<ApiResponse<PagedResult<ContractListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public ContractStatus? Status { get; init; }
        public ContractType? ContractType { get; init; }
        public Guid? AgentId { get; init; }
        public Guid? ClientId { get; init; }
        public Guid? PropertyId { get; init; }
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}