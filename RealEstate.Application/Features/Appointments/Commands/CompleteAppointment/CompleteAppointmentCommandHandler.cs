using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Appointments.Commands.CompleteAppointment
{
    public sealed class CompleteAppointmentCommandHandler
        : IRequestHandler<CompleteAppointmentCommand, ApiResponse<bool>>
    {
        private readonly IAppointmentRepository _appointments;
        private readonly IUnitOfWork _uow;

        public CompleteAppointmentCommandHandler(
            IAppointmentRepository appointments,
            IUnitOfWork uow)
        {
            _appointments = appointments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            CompleteAppointmentCommand cmd,
            CancellationToken ct)
        {
            var appointment = await _appointments.Query()
                .FirstOrDefaultAsync(a => a.Id == cmd.Id
                                       && a.CompanyId == cmd.CompanyId, ct);

            if (appointment is null)
                throw new NotFoundException("الموعد", cmd.Id);

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new ConflictException(
                    "يمكن إتمام المواعيد المؤكدة فقط");

            appointment.Status = AppointmentStatus.Done;
            appointment.ActualVisitAt = cmd.ActualVisitAt;
            appointment.Result = cmd.Result;
            appointment.Feedback = cmd.Feedback;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إتمام الموعد بنجاح");
        }
    }
}