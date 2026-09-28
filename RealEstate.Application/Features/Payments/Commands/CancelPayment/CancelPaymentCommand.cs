using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Commands.CancelPayment
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class CancelPaymentCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public string? Notes { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}