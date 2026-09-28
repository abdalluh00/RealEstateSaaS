using MediatR;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Queries.GetPagedAppointments
{
    public sealed class GetPagedAppointmentsQueryHandler
        : IRequestHandler<GetPagedAppointmentsQuery, ApiResponse<PagedResult<AppointmentListDto>>>
    {
        private readonly IAppointmentRepository _appointments;

        public GetPagedAppointmentsQueryHandler(IAppointmentRepository appointments)
            => _appointments = appointments;

        public async Task<ApiResponse<PagedResult<AppointmentListDto>>> Handle(
            GetPagedAppointmentsQuery query,
            CancellationToken ct)
        {
            var result = await _appointments.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                agentId: query.AgentId,
                clientId: query.ClientId,
                propertyId: query.PropertyId,
                status: query.Status,
                from: query.From,
                to: query.To,
                ct: ct);

              return ApiResponse<PagedResult<AppointmentListDto>>.Ok(result, "Success");
        }
    }
}