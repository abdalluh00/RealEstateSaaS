using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Commands.CreateClient
{
    [Authorize(Roles = $"{Roles.Agent}")]
    public sealed class CreateClientCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? NationalId { get; init; }
        public string? Nationality { get; init; }
        public LeadSource? Source { get; init; }
        public Guid? AssignedAgentId { get; init; }
        public string? Notes { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}