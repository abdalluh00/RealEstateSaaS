using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    public record CreateAppointmentCommand(
     Guid PropertyId,
     Guid ClientId,
     Guid AgentId,
     DateTime ScheduledAt,
     string? Notes
 ) : IRequest<ApiResponse<Guid>>;
}
