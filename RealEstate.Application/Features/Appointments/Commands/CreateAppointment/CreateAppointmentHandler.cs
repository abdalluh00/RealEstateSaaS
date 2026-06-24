using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentHandler
        : IRequestHandler<CreateAppointmentCommand, ApiResponse<Guid>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;

        public CreateAppointmentHandler(
            IAppointmentRepository appointmentRepository,
            IPropertyRepository propertyRepository,
            IClientRepository clientRepository,
            IUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _propertyRepository = propertyRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            // 1) Validate property exists
            var propertyExists = await _propertyRepository.ExistsAsync(request.PropertyId);
            if (!propertyExists)
                return ApiResponse<Guid>.Fail("العقار غير موجود");

            // 2) Validate client exists
            var clientExists = await _clientRepository.ExistsAsync(request.ClientId);
            if (!clientExists)
                return ApiResponse<Guid>.Fail("العميل غير موجود");

            // 3) Validate agent exists
            var agentExists = await _userRepository.ExistsAsync(request.AgentId);
            if (!agentExists)
                return ApiResponse<Guid>.Fail("الوسيط غير موجود");

            // 4) Validate appointment conflict for the same agent at the same time
            var hasConflict = await _appointmentRepository.HasConflictAsync(
                request.AgentId,
                request.ScheduledAt);

            if (hasConflict)
                return ApiResponse<Guid>.Fail("يوجد موعد آخر للوسيط في نفس الوقت");

            // 5) Create appointment
            var appointment = new Appointment
            {
                PropertyId = request.PropertyId,
                ClientId = request.ClientId,
                AgentId = request.AgentId,
                ScheduledAt = request.ScheduledAt,
                Notes = request.Notes,
                Status = AppointmentStatus.Pending
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(appointment.Id, "تم إنشاء الموعد بنجاح");
        }
    }
}