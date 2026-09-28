using MediatR;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Lands.Queries.GetLandDetail
{
    public sealed class GetLandDetailQueryHandler
        : IRequestHandler<GetLandDetailQuery, ApiResponse<LandDetailDto>>
    {
        private readonly ILandRepository _lands;

        public GetLandDetailQueryHandler(ILandRepository lands) => _lands = lands;

        public async Task<ApiResponse<LandDetailDto>> Handle(
            GetLandDetailQuery query,
            CancellationToken ct)
        {
            var land = await _lands.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (land is null)
                throw new NotFoundException("الأرض", query.Id);

            return ApiResponse<LandDetailDto>.Ok(land, "تم جلب بيانات الأرض بنجاح");
        }
    }
}