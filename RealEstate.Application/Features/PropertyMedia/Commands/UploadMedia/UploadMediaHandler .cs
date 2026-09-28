using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    public sealed class UploadPropertyMediaCommandHandler
        : IRequestHandler<UploadPropertyMediaCommand, ApiResponse<Guid>>
    {
        private readonly IPropertyMediaRepository _mediaRepo;
        private readonly IPropertyRepository _properties;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public UploadPropertyMediaCommandHandler(
            IPropertyMediaRepository mediaRepo,
            IPropertyRepository properties,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _mediaRepo = mediaRepo;
            _properties = properties;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            UploadPropertyMediaCommand cmd,
            CancellationToken ct)
        {
            // ── Property must exist ───────────────────────
            var propertyExists = await _properties.ExistsAsync(
                cmd.PropertyId, cmd.CompanyId, ct);

            if (!propertyExists)
                throw new NotFoundException("العقار", cmd.PropertyId);

            // ── Detect media type from extension ──────────
            var ext = Path.GetExtension(cmd.File.FileName).ToLowerInvariant();
            var mediaType = ext == ".mp4"
                ? MediaType.Video
                : MediaType.Image;

            // ── Save file to disk ─────────────────────────
            var folder = $"properties/{cmd.PropertyId}/media";
            var url = await _storage.SaveAsync(
                cmd.File.OpenReadStream(),
                cmd.File.FileName,
                folder,
                ct);

            // ── First media auto-becomes cover ────────────
            var hasCover = await _mediaRepo.HasCoverAsync(cmd.PropertyId, ct);

            var media = new Domain.Entities.Properties.PropertyMedia
            {
                PropertyId = cmd.PropertyId,
                CompanyId = cmd.CompanyId,
                MediaUrl = url,
                MediaType = mediaType,
                Title = cmd.Title,
                Description = cmd.Description,
                SortOrder = cmd.SortOrder,
                IsCover = !hasCover, // first upload = cover
                FileName = cmd.File.FileName,
                FileSizeInBytes = cmd.File.Length,
                MimeType = cmd.File.ContentType
            };

            _mediaRepo.Add(media);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(media.Id, "تم رفع الصورة بنجاح");
        }
    }
}