using MediatR;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Queries.GetTodayAppointments
{
    public sealed class GetTodayAppointmentsQueryHandler
        : IRequestHandler<GetTodayAppointmentsQuery,
            ApiResponse<IReadOnlyList<AppointmentListDto>>>
    {
        private readonly IAppointmentRepository _appointments;

        public GetTodayAppointmentsQueryHandler(IAppointmentRepository appointments)
            => _appointments = appointments;

        public async Task<ApiResponse<IReadOnlyList<AppointmentListDto>>> Handle(
            GetTodayAppointmentsQuery query,
            CancellationToken ct)
        {
            var result = await _appointments.GetTodayAsync(
                query.CompanyId, query.AgentId, ct);

            return ApiResponse<IReadOnlyList<AppointmentListDto>>.Ok(result, "Success");
        }
    }
}