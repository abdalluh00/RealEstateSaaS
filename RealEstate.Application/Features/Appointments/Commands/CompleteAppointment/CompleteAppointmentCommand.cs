using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Commands.CompleteAppointment
{
    [Authorize(Roles = $"{Roles.Agent}")]
    public sealed class CompleteAppointmentCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public DateTime ActualVisitAt { get; init; }
        public AppointmentResult Result { get; init; }
        public string? Feedback { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}