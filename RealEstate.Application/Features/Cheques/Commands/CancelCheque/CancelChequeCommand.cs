using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Commands.CancelCheque
{
    public sealed record CancelChequeCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }

        public string? Reason { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}