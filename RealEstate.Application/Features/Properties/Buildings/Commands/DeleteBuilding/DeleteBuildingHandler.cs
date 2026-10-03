using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Buildings.Commands.DeleteBuilding
{
    public sealed class DeleteBuildingCommandHandler
        : IRequestHandler<DeleteBuildingCommand, ApiResponse<bool>>
    {
        private readonly IBuildingRepository _buildings;
        private readonly IUnitOfWork _uow;

        public DeleteBuildingCommandHandler(
            IBuildingRepository buildings,
            IUnitOfWork uow)
        {
            _buildings = buildings;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteBuildingCommand cmd,
            CancellationToken ct)
        {
            var building = await _buildings.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (building is null)
                throw new NotFoundException("العمارة", cmd.Id);

            // ── Block delete if building has units ────────
            var hasUnits = await _buildings.HasUnitsAsync(cmd.Id, ct);

            if (hasUnits)
                throw new ConflictException(
                    "لا يمكن حذف العمارة لأن بها وحدات مرتبطة بها — قم بحذف الوحدات أولاً");

            _buildings.SoftDelete(building);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف العمارة بنجاح");
        }
    }
}