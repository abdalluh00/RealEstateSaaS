using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Queries.GetTodayAppointments
{
    public sealed class GetTodayAppointmentsQuery
        : IRequest<ApiResponse<IReadOnlyList<AppointmentListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid? AgentId { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}