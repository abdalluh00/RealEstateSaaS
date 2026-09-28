using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Lands.Commands.DeleteLand
{
    public sealed class DeleteLandCommandHandler
        : IRequestHandler<DeleteLandCommand, ApiResponse<bool>>
    {
        private readonly ILandRepository _lands;
        private readonly IUnitOfWork _uow;

        public DeleteLandCommandHandler(ILandRepository lands, IUnitOfWork uow)
        {
            _lands = lands;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteLandCommand cmd,
            CancellationToken ct)
        {
            var land = await _lands.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (land is null)
                throw new NotFoundException("الأرض", cmd.Id);

            _lands.SoftDelete(land);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الأرض بنجاح");
        }
    }
}