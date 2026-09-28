using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Queries.GetContractCheques
{
    public sealed record GetContractChequesQuery
        : IRequest<ApiResponse<IReadOnlyList<ChequeListDto>>>, IAutoTenantRequest
    {
        public Guid ContractId { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}