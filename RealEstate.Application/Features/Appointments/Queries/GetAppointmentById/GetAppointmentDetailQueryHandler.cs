using MediatR;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Appointments.Queries.GetAppointmentDetail
{
    public sealed class GetAppointmentDetailQueryHandler
        : IRequestHandler<GetAppointmentDetailQuery, ApiResponse<AppointmentDetailDto>>
    {
        private readonly IAppointmentRepository _appointments;

        public GetAppointmentDetailQueryHandler(IAppointmentRepository appointments)
            => _appointments = appointments;

        public async Task<ApiResponse<AppointmentDetailDto>> Handle(
            GetAppointmentDetailQuery query,
            CancellationToken ct)
        {
            var appointment = await _appointments.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (appointment is null)
                throw new NotFoundException("الموعد", query.Id);

            return ApiResponse<AppointmentDetailDto>.Ok(appointment, "Success");
        }
    }
}