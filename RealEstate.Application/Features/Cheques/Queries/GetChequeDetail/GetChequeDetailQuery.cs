using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Queries.GetChequeDetail
{
    public sealed record GetChequeDetailQuery
        : IRequest<ApiResponse<ChequeDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}