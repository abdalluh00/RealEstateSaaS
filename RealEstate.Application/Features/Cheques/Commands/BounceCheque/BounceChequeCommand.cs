using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Commands.BounceCheque
{
    public sealed record BounceChequeCommand
        : IRequest<ApiResponse<Guid>> , IAutoTenantRequest
    {
        public Guid Id { get; init; }

        public string BounceReason { get; init; } = string.Empty;

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}