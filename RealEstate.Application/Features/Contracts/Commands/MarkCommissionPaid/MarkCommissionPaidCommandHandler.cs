using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Contracts.Commands.MarkCommissionPaid
{
    public sealed class MarkCommissionPaidCommandHandler
        : IRequestHandler<MarkCommissionPaidCommand, ApiResponse<bool>>
    {
        private readonly IContractRepository _contracts;
        private readonly IUnitOfWork _uow;

        public MarkCommissionPaidCommandHandler(
            IContractRepository contracts,
            IUnitOfWork uow)
        {
            _contracts = contracts;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            MarkCommissionPaidCommand cmd,
            CancellationToken ct)
        {
            var contract = await _contracts.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (contract is null)
                throw new NotFoundException("العقد", cmd.Id);

            if (contract.CommissionStatus == CommissionStatus.Paid)
                throw new ConflictException("العمولة مدفوعة بالفعل");

            contract.CommissionStatus = CommissionStatus.Paid;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تسجيل دفع العمولة بنجاح");
        }
    }
}