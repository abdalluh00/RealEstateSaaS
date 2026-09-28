using MediatR;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Lands.Queries.GetPagedLands
{
    public sealed class GetPagedLandsQueryHandler
        : IRequestHandler<GetPagedLandsQuery, ApiResponse<PagedResult<LandListDto>>>
    {
        private readonly ILandRepository _lands;

        public GetPagedLandsQueryHandler(ILandRepository lands) => _lands = lands;

        public async Task<ApiResponse<PagedResult<LandListDto>>> Handle(
            GetPagedLandsQuery query,
            CancellationToken ct)
        {
            var result = await _lands.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                purpose: query.Purpose,
                zoningType: query.ZoningType,
                ct: ct);

            return ApiResponse<PagedResult<LandListDto>>.Ok(result, "تم جلب بيانات الأراضي بنجاح");

        }
    }
}