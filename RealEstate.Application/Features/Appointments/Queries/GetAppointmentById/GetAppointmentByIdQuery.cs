using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Queries.GetAppointmentDetail
{
    public sealed class GetAppointmentDetailQuery
        : IRequest<ApiResponse<AppointmentDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}