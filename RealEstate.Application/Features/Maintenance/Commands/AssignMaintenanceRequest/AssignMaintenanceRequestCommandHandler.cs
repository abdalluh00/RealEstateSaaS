using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Commands.AssignMaintenanceRequest
{
    public sealed class AssignMaintenanceRequestCommandHandler
        : IRequestHandler<AssignMaintenanceRequestCommand, ApiResponse<bool>>
    {
        private readonly IMaintenanceRepository _maintenance;
        private readonly IUnitOfWork _uow;

        public AssignMaintenanceRequestCommandHandler(
            IMaintenanceRepository maintenance,
            IUnitOfWork uow)
        {
            _maintenance = maintenance;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            AssignMaintenanceRequestCommand cmd,
            CancellationToken ct)
        {
            var request = await _maintenance.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (request is null)
                throw new NotFoundException("طلب الصيانة", cmd.Id);

            if (request.Status == MaintenanceStatus.Done
             || request.Status == MaintenanceStatus.Cancelled)
                throw new ConflictException("لا يمكن تعيين طلب منتهٍ أو ملغى");

            request.AssignedToId = cmd.AssignedToId;
            request.Status = MaintenanceStatus.InProgress;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تعيين الطلب بنجاح");
        }
    }
}