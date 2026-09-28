using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class MarkPaymentPaidCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public DateTime PaidDate { get; init; }
        public PaymentMethod PaymentMethod { get; init; }
        public string? Reference { get; init; }
        public string? Notes { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}