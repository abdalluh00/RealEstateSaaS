using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Commands.UpdateClient
{
    [Authorize(Roles = $"{Roles.Agent}")]
    public sealed class UpdateClientCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }

        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? NationalId { get; init; }
        public string? Nationality { get; init; }
        public LeadSource? Source { get; init; }
        public bool IsActive { get; init; }
        public string? Notes { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}