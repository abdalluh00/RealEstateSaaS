using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Queries.GetContractDetail
{
    public sealed class GetContractDetailQuery
        : IRequest<ApiResponse<ContractDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}