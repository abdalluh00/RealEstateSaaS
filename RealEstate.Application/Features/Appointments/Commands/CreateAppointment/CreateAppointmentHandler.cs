using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    public sealed class CreateAppointmentCommandHandler
        : IRequestHandler<CreateAppointmentCommand, ApiResponse<Guid>>
    {
        private readonly IAppointmentRepository _appointments;
        private readonly IPropertyRepository _properties;
        private readonly IUnitOfWork _uow;

        public CreateAppointmentCommandHandler(
            IAppointmentRepository appointments,
            IPropertyRepository properties,
            IUnitOfWork uow)
        {
            _appointments = appointments;
            _properties = properties;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateAppointmentCommand cmd,
            CancellationToken ct)
        {
            // ── Property must exist and be available ──────
            var propertyAvailable = await _properties.IsAvailableAsync(
                cmd.PropertyId, ct);

            if (!propertyAvailable)
                throw new ConflictException("العقار غير متاح للحجز");

            // ── Agent must not have conflicting appointment ─
            var hasConflict = await _appointments.HasConflictAsync(
                agentId: cmd.AgentId,
                scheduledAt: cmd.ScheduledAt,
                durationMinutes: cmd.DurationMinutes,
                ct: ct);

            if (hasConflict)
                throw new ConflictException(
                    "الوكيل لديه موعد آخر في نفس الوقت");

            var appointment = new Appointment
            {
                PropertyId = cmd.PropertyId,
                ClientId = cmd.ClientId,
                AgentId = cmd.AgentId,
                CompanyId = cmd.CompanyId,
                ScheduledAt = cmd.ScheduledAt,
                DurationMinutes = cmd.DurationMinutes,
                Notes = cmd.Notes,
                Status = AppointmentStatus.Pending
            };

            _appointments.Add(appointment);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(appointment.Id, "تم إنشاء الموعد بنجاح");
        }
    }
}