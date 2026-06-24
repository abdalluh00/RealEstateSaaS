using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Appointments.Commands.UpdateAppointmentStatus
{
    public class UpdateAppointmentStatusHandler
    : IRequestHandler<UpdateAppointmentStatusCommand, ApiResponse<bool>>
    {
        private readonly IAppointmentRepository _repo;

        public UpdateAppointmentStatusHandler(IAppointmentRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            UpdateAppointmentStatusCommand request,
            CancellationToken ct)
        {
            var appointment = await _repo.GetByIdAsync(request.Id);

            if (appointment is null)
                throw new NotFoundException("الموعد", request.Id);

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new ConflictException("لا يمكن تعديل موعد ملغي");

            appointment.Status = request.Status;
            appointment.Feedback = request.Feedback;

            _repo.Update(appointment);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث حالة الموعد بنجاح");
        }
    }
}
