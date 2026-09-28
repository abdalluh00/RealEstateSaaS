using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Queries.GetPagedCheques
{
    public sealed record GetPagedChequesQuery
        : IRequest<ApiResponse<PagedResult<ChequeListDto>>>, IAutoTenantRequest
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;

        public Guid? ContractId { get; init; }

        public ChequeStatus? Status { get; init; }

        public DateTime? From { get; init; }
        public DateTime? To { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}