using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Commands.CancelMaintenanceRequest
{
    public sealed class CancelMaintenanceRequestCommandHandler
        : IRequestHandler<CancelMaintenanceRequestCommand, ApiResponse<bool>>
    {
        private readonly IMaintenanceRepository _maintenance;
        private readonly IUnitOfWork _uow;

        public CancelMaintenanceRequestCommandHandler(
            IMaintenanceRepository maintenance,
            IUnitOfWork uow)
        {
            _maintenance = maintenance;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            CancelMaintenanceRequestCommand cmd,
            CancellationToken ct)
        {
            var request = await _maintenance.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (request is null)
                throw new NotFoundException("طلب الصيانة", cmd.Id);

            if (request.Status is MaintenanceStatus.Done
                                or MaintenanceStatus.Cancelled)
                throw new ConflictException("لا يمكن إلغاء هذا الطلب");

            request.Status = MaintenanceStatus.Cancelled;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إلغاء طلب الصيانة بنجاح");
        }
    }
}