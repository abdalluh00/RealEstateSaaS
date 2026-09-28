using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Commands.ReassignAgent
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class ReassignAgentCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public Guid? NewAgentId { get; init; }
        // null = unassign agent
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}