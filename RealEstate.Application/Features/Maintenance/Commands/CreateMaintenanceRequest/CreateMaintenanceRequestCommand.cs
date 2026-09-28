using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Commands.CreateMaintenanceRequest
{
    [Authorize(Roles = $"{Roles.Agent},{Roles.Admin}")]
    public sealed class CreateMaintenanceRequestCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public MaintenanceCategory Category { get; init; }
        public MaintenancePriority Priority { get; init; }
        public Guid? PropertyId { get; init; }
        public Guid? ClientId { get; init; }
        public Guid? AssignedToId { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}