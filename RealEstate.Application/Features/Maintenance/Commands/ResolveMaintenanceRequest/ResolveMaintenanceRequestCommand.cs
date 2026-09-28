using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Commands.ResolveMaintenanceRequest
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class ResolveMaintenanceRequestCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public string ResolutionNotes { get; init; } = string.Empty;
        public decimal? Cost { get; init; }
        public string? ContractorName { get; init; }
        public string? ContractorPhone { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}