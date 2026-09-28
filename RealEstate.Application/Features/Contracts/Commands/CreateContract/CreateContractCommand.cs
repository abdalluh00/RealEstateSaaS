using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Commands.CreateContract
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class CreateContractCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public Guid PropertyId { get; init; }
        public Guid ClientId { get; init; }
        public Guid AgentId { get; init; }

        // ── Contract Info ─────────────────────────────────
        public ContractType ContractType { get; init; }
        public decimal Amount { get; init; }
        public decimal? SecurityDeposit { get; init; }
        public PaymentCycle? PaymentCycle { get; init; }
        public PaymentMethod? PaymentMethod { get; init; }

        // ── Commission ────────────────────────────────────
        public CommissionType CommissionType { get; init; }
        public decimal Commission { get; init; }

        // ── Dates ─────────────────────────────────────────
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }

        public string? Notes { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}