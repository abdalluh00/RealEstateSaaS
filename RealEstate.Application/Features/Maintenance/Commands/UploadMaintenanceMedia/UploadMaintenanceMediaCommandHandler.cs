using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Commands.UploadMaintenanceMedia
{
    public sealed class UploadMaintenanceMediaCommandHandler
        : IRequestHandler<UploadMaintenanceMediaCommand, ApiResponse<Guid>>
    {
        private readonly IMaintenanceMediaRepository _mediaRepo;
        private readonly IMaintenanceRepository _maintenance;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public UploadMaintenanceMediaCommandHandler(
            IMaintenanceMediaRepository mediaRepo,
            IMaintenanceRepository maintenance,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _mediaRepo = mediaRepo;
            _maintenance = maintenance;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            UploadMaintenanceMediaCommand cmd,
            CancellationToken ct)
        {
            var request = await _maintenance.GetByIdForCommandAsync(
                cmd.MaintenanceRequestId, cmd.CompanyId, ct);

            if (request is null)
                throw new NotFoundException("طلب الصيانة", cmd.MaintenanceRequestId);

            var ext = Path.GetExtension(cmd.File.FileName).ToLowerInvariant();
            var mediaType = ext == ".mp4"
                ? MaintenanceMediaType.Video
                : MaintenanceMediaType.Image;

            var folder = $"maintenance/{cmd.MaintenanceRequestId}";
            var url = await _storage.SaveAsync(
                cmd.File.OpenReadStream(),
                cmd.File.FileName,
                folder,
                ct);

            var media = new MaintenanceMedia
            {
                MaintenanceRequestId = cmd.MaintenanceRequestId,
                CompanyId = cmd.CompanyId,
                FileUrl = url,
                FileName = cmd.File.FileName,
                FileSizeInBytes = cmd.File.Length,
                MimeType = cmd.File.ContentType,
                MediaType = mediaType,
                Stage = cmd.Stage,
                UploadedByType = cmd.UploadedByType,
                UploadedById = cmd.UploadedById
            };

            _mediaRepo.Add(media);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(media.Id, "تم رفع الصورة بنجاح");
        }
    }
}