using MediatR;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Application.Features.Properties.Apartments.Queries.GetApartmentById;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Apartments.Queries.GetApartmentDetail
{
    public sealed class GetApartmentDetailQueryHandler
        : IRequestHandler<GetApartmentDetailQuery, ApiResponse<ApartmentDetailDto>>
    {
        private readonly IApartmentRepository _apartments;

        public GetApartmentDetailQueryHandler(IApartmentRepository apartments)
            => _apartments = apartments;

        public async Task<ApiResponse<ApartmentDetailDto>> Handle(
            GetApartmentDetailQuery query,
            CancellationToken ct)
        {
            var apartment = await _apartments.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (apartment is null)
                throw new NotFoundException("الشقة", query.Id);

            return ApiResponse<ApartmentDetailDto>.Ok(apartment);
        }
    }
}