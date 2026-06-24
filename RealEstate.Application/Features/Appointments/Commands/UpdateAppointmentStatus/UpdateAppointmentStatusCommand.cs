using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Appointments.Commands.UpdateAppointmentStatus
{
    public record UpdateAppointmentStatusCommand(
     Guid Id,
     AppointmentStatus Status,
     string? Feedback
 ) : IRequest<ApiResponse<bool>>;
}
