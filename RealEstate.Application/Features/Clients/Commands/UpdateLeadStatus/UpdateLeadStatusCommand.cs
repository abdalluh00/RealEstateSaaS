using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Commands.UpdateLeadStatus
{
    [Authorize(Roles = $"{Roles.Agent}")]
    public sealed class UpdateLeadStatusCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public LeadStatus LeadStatus { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}