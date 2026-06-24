using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Appointments.Commands.DeleteAppointment
{
    public record DeleteAppointmentCommand(Guid Id) : IRequest<ApiResponse<bool>>;

}
