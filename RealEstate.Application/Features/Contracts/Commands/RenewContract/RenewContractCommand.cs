using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Commands.RenewContract
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class RenewContractCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }              // old contract
        public Guid CompanyId { get; private set; }
        public DateTime NewStartDate { get; init; }
        public DateTime NewEndDate { get; init; }
        public decimal NewAmount { get; init; }
        public string? Notes { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}