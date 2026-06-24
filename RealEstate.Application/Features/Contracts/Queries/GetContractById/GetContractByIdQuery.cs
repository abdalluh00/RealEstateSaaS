using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Queries.GetContractById
{
    public record GetContractByIdQuery(Guid Id)
         : IRequest<ApiResponse<ContractDetailDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }

}
