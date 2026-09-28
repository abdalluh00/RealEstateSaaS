using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    [Authorize(Roles = $"{Roles.Agent}")]
    public sealed class CreateAppointmentCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid PropertyId { get; init; }
        public Guid ClientId { get; init; }
        public Guid AgentId { get; init; }
        public DateTime ScheduledAt { get; init; }
        public int DurationMinutes { get; init; } = 30;
        public string? Notes { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}