using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Commands.AssignMaintenanceRequest
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class AssignMaintenanceRequestCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public Guid AssignedToId { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}