using MediatR;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments
{
    public sealed class GetPagedApartmentsQueryHandler
         : IRequestHandler<GetPagedApartmentsQuery, ApiResponse<PagedResult<ApartmentListDto>>>
    {
        private readonly IApartmentRepository _apartments;

        public GetPagedApartmentsQueryHandler(IApartmentRepository apartments)
            => _apartments = apartments;

        public async Task<ApiResponse<PagedResult<ApartmentListDto>>> Handle(
            GetPagedApartmentsQuery query,
            CancellationToken ct)
        {
            var result = await _apartments.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                purpose: query.Purpose,
                minBedrooms: query.MinBedrooms,
                maxBedrooms: query.MaxBedrooms,
                furnishedStatus: query.FurnishedStatus,
                ct: ct);

            return ApiResponse<PagedResult<ApartmentListDto>>.Ok(result);
        }
    }
}
