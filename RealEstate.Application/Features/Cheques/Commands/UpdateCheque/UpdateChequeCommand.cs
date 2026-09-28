using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Commands.UpdateCheque
{
    public sealed record UpdateChequeCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }

        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;

        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }

        public int ChequeOrder { get; init; }

        public string? Notes { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}