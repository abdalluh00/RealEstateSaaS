using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Contracts.Commands.RenewContract
{
    public sealed class RenewContractCommandHandler
        : IRequestHandler<RenewContractCommand, ApiResponse<Guid>>
    {
        private readonly IContractRepository _contracts;
        private readonly IUnitOfWork _uow;
        private readonly IContractNumberGenerator _numberGenerator;

        public RenewContractCommandHandler(
            IContractRepository contracts,
            IUnitOfWork uow,
            IContractNumberGenerator numberGenerator)
        {
            _contracts = contracts;
            _uow = uow;
            _numberGenerator = numberGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            RenewContractCommand cmd,
            CancellationToken ct)
        {
            var old = await _contracts.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (old is null)
                throw new NotFoundException("العقد", cmd.Id);

            if (old.ContractStatus is not (ContractStatus.Active
                                        or ContractStatus.Expired))
                throw new ConflictException(
                    "يمكن تجديد العقود النشطة أو المنتهية فقط");

            // ── Mark old contract as renewed ──────────────
            old.ContractStatus = ContractStatus.Renewed;

            // ── Create new contract ───────────────────────
            var newNumber = await _numberGenerator.GenerateAsync(
                cmd.CompanyId, ct);

            var newContract = new Contract
            {
                ContractNumber = newNumber,
                ContractType = old.ContractType,
                ContractStatus = ContractStatus.Active,
                Amount = cmd.NewAmount,
                SecurityDeposit = old.SecurityDeposit,
                PaymentCycle = old.PaymentCycle,
                PaymentMethod = old.PaymentMethod,
                CommissionType = old.CommissionType,
                Commission = old.Commission,
                CommissionStatus = CommissionStatus.Pending,
                StartDate = cmd.NewStartDate,
                EndDate = cmd.NewEndDate,
                Notes = cmd.Notes,
                PropertyId = old.PropertyId,
                ClientId = old.ClientId,
                AgentId = old.AgentId,
                CompanyId = old.CompanyId,
                RenewedFromContractId = old.Id
            };

            _contracts.Add(newContract);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(newContract.Id, "تم تجديد العقد بنجاح");
        }
    }
}