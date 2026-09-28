using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia
{
    public sealed class DeleteMediaCommandHandler
        : IRequestHandler<DeleteMediaCommand, ApiResponse<bool>>
    {
        private readonly IPropertyMediaRepository _mediaRepo;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public DeleteMediaCommandHandler(
            IPropertyMediaRepository mediaRepo,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _mediaRepo = mediaRepo;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteMediaCommand cmd,
            CancellationToken ct)
        {
            var media = await _mediaRepo.GetByIdForCommandAsync(
                cmd.MediaId, cmd.CompanyId, ct);

            if (media is null || media.PropertyId != cmd.PropertyId)
                throw new NotFoundException("الصورة", cmd.MediaId);

            // ── Delete physical file first ────────────────
            await _storage.DeleteAsync(media.MediaUrl, ct);

            // ── Remove DB record ──────────────────────────
            _mediaRepo.HardDelete(media);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الصورة بنجاح");
        }
    }
}