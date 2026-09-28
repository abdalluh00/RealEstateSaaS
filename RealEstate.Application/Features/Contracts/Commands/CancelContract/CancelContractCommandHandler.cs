using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Contracts.Commands.CancelContract
{
    public sealed class CancelContractCommandHandler
        : IRequestHandler<CancelContractCommand, ApiResponse<bool>>
    {
        private readonly IContractRepository _contracts;
        private readonly IPropertyRepository _properties;
        private readonly IUnitOfWork _uow;

        public CancelContractCommandHandler(
            IContractRepository contracts,
            IPropertyRepository properties,
            IUnitOfWork uow)
        {
            _contracts = contracts;
            _properties = properties;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            CancelContractCommand cmd,
            CancellationToken ct)
        {
            var contract = await _contracts.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (contract is null)
                throw new NotFoundException("العقد", cmd.Id);

            if (contract.ContractStatus != ContractStatus.Active)
                throw new ConflictException("يمكن إلغاء العقود النشطة فقط");

            contract.ContractStatus = ContractStatus.Cancelled;
            contract.CancellationReason = cmd.CancellationReason;
            contract.CancelledAt = DateTime.UtcNow;

            // ── Reset property status to Available ────────
            var property = await _properties.GetByIdForDeleteAsync(
                contract.PropertyId, cmd.CompanyId, ct);

            if (property is not null)
                property.PropertyStatus = PropertyStatus.Available;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إلغاء العقد بنجاح");
        }
    }
}