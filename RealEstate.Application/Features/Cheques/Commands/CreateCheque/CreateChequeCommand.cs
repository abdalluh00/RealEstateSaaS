using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Commands.CreateCheque
{
    [Authorize(Policy = Policies.AgentAndUp)]
    public sealed record CreateChequeCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;

        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }

        public int ChequeOrder { get; init; }

        public Guid ContractId { get; init; }

        public string? Notes { get; init; }

        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}