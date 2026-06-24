using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyMedia.Commands.SetCover
{
    public class SetCoverHandler : IRequestHandler<SetCoverCommand, ApiResponse<bool>>
    {
        private readonly IPropertyMediaRepository _repo;

        public SetCoverHandler(IPropertyMediaRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            SetCoverCommand request,
            CancellationToken ct)
        {
            var media = await _repo.GetByIdAsync(request.MediaId);

            if (media is null)
                throw new NotFoundException("الملف", request.MediaId);

            // احذف الغلاف القديم
            var oldCover = await _repo.GetCoverAsync(media.PropertyId);

            if (oldCover is not null)
            {
                oldCover.IsCover = false;
                _repo.Update(oldCover);
            }

            // اجعل هذا الملف غلافاً
            media.IsCover = true;
            _repo.Update(media);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تعيين صورة الغلاف بنجاح");
        }
    }
}
