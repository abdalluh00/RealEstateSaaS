using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public sealed class ConfirmAppointmentCommandHandler
        : IRequestHandler<ConfirmAppointmentCommand, ApiResponse<bool>>
    {
        private readonly IAppointmentRepository _appointments;
        private readonly IUnitOfWork _uow;

        public ConfirmAppointmentCommandHandler(
            IAppointmentRepository appointments,
            IUnitOfWork uow)
        {
            _appointments = appointments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            ConfirmAppointmentCommand cmd,
            CancellationToken ct)
        {
            var appointment = await _appointments.Query()
                .FirstOrDefaultAsync(a => a.Id == cmd.Id
                                       && a.CompanyId == cmd.CompanyId, ct);

            if (appointment is null)
                throw new NotFoundException("الموعد", cmd.Id);

            if (appointment.Status != AppointmentStatus.Pending)
                throw new ConflictException("يمكن تأكيد المواعيد المعلقة فقط");

            appointment.Status = AppointmentStatus.Confirmed;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تأكيد الموعد بنجاح");
        }
    }
}