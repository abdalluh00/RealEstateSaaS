using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Contracts.Queries.GetContracts
{
    public record GetContractsQuery()
        : IRequest<ApiResponse<List<ContractDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }


}
