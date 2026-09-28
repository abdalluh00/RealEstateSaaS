using MediatR;
using RealEstate.Application.DTOs.Owner;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Owners.Queries.GetPagedOwners
{
    public sealed class GetPagedOwnersQueryHandler
        : IRequestHandler<GetPagedOwnersQuery, ApiResponse<PagedResult<OwnerListDto>>>
    {
        private readonly IOwnerRepository _owners;

        public GetPagedOwnersQueryHandler(IOwnerRepository owners)
            => _owners = owners;

        public async Task<ApiResponse<PagedResult<OwnerListDto>>> Handle(
            GetPagedOwnersQuery query,
            CancellationToken ct)
        {
            var result = await _owners.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                isActive: query.IsActive,
                ownerType: query.OwnerType,
                search: query.Search,
                ct: ct);

            return ApiResponse<PagedResult<OwnerListDto>>.Ok(result, "تم استرجاع قائمة الملاك بنجاح");
        }
    }
}