
using MediatR;
using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Offices.Query.GetOfficePages
{
    public sealed class GetOfficePagesQueryHandler : IRequestHandler<GetOfficePagesQuery, ApiResponse<PagedResult<OfficeListDto>>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficePagesQueryHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public async Task<ApiResponse<PagedResult<OfficeListDto>>> Handle(GetOfficePagesQuery request, CancellationToken cancellationToken)
        {
            var result = await _officeRepository.GetPagedAsync(request.CompanyId, request.Page, request.PageSize, request.status, 
                request.purpose, request.FurnishedStatus, cancellationToken);

            return ApiResponse<PagedResult<OfficeListDto>>.Ok(result, "Offices retrieved successfully");
        }
    }
}
