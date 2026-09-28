using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyMedia.Commands.SetCover
{
    public sealed class SetCoverCommandHandler
        : IRequestHandler<SetCoverCommand, ApiResponse<bool>>
    {
        private readonly IPropertyMediaRepository _mediaRepo;
        private readonly IUnitOfWork _uow;

        public SetCoverCommandHandler(
            IPropertyMediaRepository mediaRepo,
            IUnitOfWork uow)
        {
            _mediaRepo = mediaRepo;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            SetCoverCommand cmd,
            CancellationToken ct)
        {
            var media = await _mediaRepo.GetByIdForCommandAsync(
                cmd.MediaId, cmd.CompanyId, ct);

            if (media is null || media.PropertyId != cmd.PropertyId)
                throw new NotFoundException("الصورة", cmd.MediaId);

            // ── Clear existing cover ──────────────────────
            await _mediaRepo.ClearCoverAsync(cmd.PropertyId, ct);

            // ── Set new cover ─────────────────────────────
            media.IsCover = true;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تعيين صورة الغلاف بنجاح");
        }
    }
}