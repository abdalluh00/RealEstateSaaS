using MediatR;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Appointments.Commands.UpdateAppointment
{
    public record UpdateAppointmentCommand(
        Guid Id,
        Guid PropertyId,
        Guid ClientId,
        Guid AgentId,
        DateTime ScheduledAt,
        string? Notes
    ) : IRequest<ApiResponse<bool>>;
}
