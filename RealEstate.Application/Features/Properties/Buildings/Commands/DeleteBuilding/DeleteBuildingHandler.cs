using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Features.Properties.Buildings.Commands.DeleteBuilding;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Building.Commands.DeleteBuilding
{
    public class DeleteBuildingHandler : IRequestHandler<DeleteBuildingCommand, ApiResponse<bool>>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IUnitOfWork _uow;

        public DeleteBuildingHandler(
            IBuildingRepository buildingRepo,
            IUnitOfWork uow)
        {
            _buildingRepo = buildingRepo;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteBuildingCommand request,
            CancellationToken ct)
        {
            // ── Fetch entity ──────────────────────────────
            var building = await _buildingRepo
                .Query()
                .FirstOrDefaultAsync(b => b.Id == request.Id
                                       && b.CompanyId == request.CompanyId, ct)
                ?? throw new NotFoundException("المبنى غير موجود");

            // ── Business rule: cannot delete if has units ─
            var hasUnits = await _buildingRepo.HasUnitsAsync(request.Id, ct);
            if (hasUnits)
                throw new ValidationException("لا يمكن حذف المبنى — يحتوي على وحدات نشطة");

            _buildingRepo.SoftDelete(building);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true);
        }
    }
}