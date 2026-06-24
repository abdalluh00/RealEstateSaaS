using MediatR;
using RealEstate.Application.Features.Properties.Apartments.DTOs;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments
{
    public class GetApartmentsHandler : IRequestHandler<GetApartmentsQuery, ApiResponse<PagedResult<ApartmentListItemDto>>>
    {
        private readonly IApartmentRepository _apartmentRepository;

        public GetApartmentsHandler(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public async Task<ApiResponse<PagedResult<ApartmentListItemDto>>> Handle(GetApartmentsQuery request, CancellationToken ct)
        {
            var result = await _apartmentRepository.GetPagedAsync(
                request.CompanyId,
                request.Page,
                request.PageSize,
                request.Search,
                request.Purpose,
                request.PropertyStatus,
                request.ParentPropertyId);

            var dto = new PagedResult<ApartmentListItemDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Items = result.Items.Select(apartment => new ApartmentListItemDto
                {
                    Id = apartment.Id,
                    PropertyCode = apartment.PropertyCode,
                    Title = apartment.Title,
                    Purpose = apartment.Purpose,
                    PropertyStatus = apartment.PropertyStatus,
                    Price = apartment.Price,
                    Area = apartment.Area,
                    City = apartment.City,
                    District = apartment.District,
                    ParentPropertyId = apartment.ParentPropertyId,
                    OwnerId = apartment.OwnerId ?? Guid.Empty,
                    AgentId = apartment.AgentId ?? Guid.Empty,
                    UnitNumber = apartment.UnitNumber,
                    Bedrooms = apartment.Bedrooms,
                    Bathrooms = apartment.Bathrooms,
                    FloorNumber = apartment.FloorNumber,
                    IsFeatured = apartment.IsFeatured,
                    CreatedAt = apartment.CreatedAt
                }).ToList()
            };

            return ApiResponse<PagedResult<ApartmentListItemDto>>.Ok(dto);
        }
    }
}
