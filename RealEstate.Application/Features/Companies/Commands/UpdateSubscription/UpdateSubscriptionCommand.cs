using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Commands.UpdateSubscription
{
    // No [Authorize] — called from payment webhook or super admin
    // Protected at controller level
    public sealed class UpdateSubscriptionCommand
        : IRequest<ApiResponse<bool>>, IAuthRequest
    {
        public Guid CompanyId { get; init; }
        public SubscriptionPlan Plan { get; init; }
        public DateTime NewExpiry { get; init; }
    }
}