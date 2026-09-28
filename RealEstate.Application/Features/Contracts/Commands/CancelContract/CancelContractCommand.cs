using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Commands.CancelContract
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class CancelContractCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public string CancellationReason { get; init; } = string.Empty;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}