using MediatR;
using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Offices.Query.GetOfficeDetailById
{
    public sealed class GetOfficeDetailQueryHandler : IRequestHandler<GetOfficeDetailQuery, ApiResponse<OfficeDetailDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficeDetailQueryHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public async Task<ApiResponse<OfficeDetailDto>> Handle(GetOfficeDetailQuery request, CancellationToken cancellationToken)
        {
            var officeDetail = await _officeRepository.GetDetailByIdAsync(request.Id, request.CompanyId);

            if(officeDetail == null)
                throw new NotFoundException($"Office with Id {request.Id} not found.");

            return ApiResponse<OfficeDetailDto>.Ok(officeDetail, "Office detail retrieved successfully.");
        }
    }
}
