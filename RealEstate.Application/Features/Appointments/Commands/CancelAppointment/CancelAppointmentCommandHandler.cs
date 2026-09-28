using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Appointments.Commands.CancelAppointment
{
    public sealed class CancelAppointmentCommandHandler
        : IRequestHandler<CancelAppointmentCommand, ApiResponse<bool>>
    {
        private readonly IAppointmentRepository _appointments;
        private readonly IUnitOfWork _uow;

        public CancelAppointmentCommandHandler(
            IAppointmentRepository appointments,
            IUnitOfWork uow)
        {
            _appointments = appointments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            CancelAppointmentCommand cmd,
            CancellationToken ct)
        {
            var appointment = await _appointments.Query()
                .FirstOrDefaultAsync(a => a.Id == cmd.Id
                                       && a.CompanyId == cmd.CompanyId, ct);

            if (appointment is null)
                throw new NotFoundException("الموعد", cmd.Id);

            if (appointment.Status is AppointmentStatus.Done
                                   or AppointmentStatus.Cancelled)
                throw new ConflictException("لا يمكن إلغاء هذا الموعد");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = cmd.CancellationReason;
            appointment.CancelledAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إلغاء الموعد بنجاح");
        }
    }
}