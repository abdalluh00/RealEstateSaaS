using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Commands.ResolveMaintenanceRequest
{
    public sealed class ResolveMaintenanceRequestCommandHandler
        : IRequestHandler<ResolveMaintenanceRequestCommand, ApiResponse<bool>>
    {
        private readonly IMaintenanceRepository _maintenance;
        private readonly IUnitOfWork _uow;

        public ResolveMaintenanceRequestCommandHandler(
            IMaintenanceRepository maintenance,
            IUnitOfWork uow)
        {
            _maintenance = maintenance;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            ResolveMaintenanceRequestCommand cmd,
            CancellationToken ct)
        {
            var request = await _maintenance.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (request is null)
                throw new NotFoundException("طلب الصيانة", cmd.Id);

            if (request.Status == MaintenanceStatus.Done)
                throw new ConflictException("الطلب محلول بالفعل");

            if (request.Status == MaintenanceStatus.Cancelled)
                throw new ConflictException("لا يمكن حل طلب ملغى");

            request.Status = MaintenanceStatus.Done;
            request.ResolutionNotes = cmd.ResolutionNotes;
            request.Cost = cmd.Cost;
            request.ContractorName = cmd.ContractorName;
            request.ContractorPhone = cmd.ContractorPhone;
            request.ResolvedAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إغلاق طلب الصيانة بنجاح");
        }
    }
}