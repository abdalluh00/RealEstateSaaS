using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Commands.DeleteMaintenanceMedia
{
    public sealed class DeleteMaintenanceMediaCommandHandler
        : IRequestHandler<DeleteMaintenanceMediaCommand, ApiResponse<bool>>
    {
        private readonly IMaintenanceMediaRepository _mediaRepo;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public DeleteMaintenanceMediaCommandHandler(
            IMaintenanceMediaRepository mediaRepo,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _mediaRepo = mediaRepo;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteMaintenanceMediaCommand cmd,
            CancellationToken ct)
        {
            var media = await _mediaRepo.GetByIdForCommandAsync(
                cmd.MediaId, cmd.CompanyId, ct);

            if (media is null)
                throw new NotFoundException("الصورة", cmd.MediaId);

            await _storage.DeleteAsync(media.FileUrl, ct);

            _mediaRepo.HardDelete(media);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الصورة بنجاح");
        }
    }
}