using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    public class UploadMediaHandler : IRequestHandler<UploadMediaCommand, ApiResponse<Guid>>
    {
        private readonly IPropertyMediaRepository _mediaRepo;
        private readonly IPropertyRepository _propertyRepo;
        private readonly IStorageService _storage;

        public UploadMediaHandler(
            IPropertyMediaRepository mediaRepo,
            IPropertyRepository propertyRepo,
            IStorageService storage)
        {
            _mediaRepo = mediaRepo;
            _propertyRepo = propertyRepo;
            _storage = storage;
        }

        public async Task<ApiResponse<Guid>> Handle(
            UploadMediaCommand request,
            CancellationToken ct)
        {
            // تحقق من العقار
            var property = await _propertyRepo.GetByIdAsync(request.PropertyId);
            if (property is null)
                throw new NotFoundException("العقار", request.PropertyId);

            // رفع الملف
            var url = await _storage.UploadAsync(
                request.FileStream,
                request.FileName,
                request.ContentType);

            // إذا هذه صورة الغلاف — احذف الغلاف القديم
            if (request.IsCover)
            {
                var oldCover = await _mediaRepo.GetCoverAsync(request.PropertyId);
                if (oldCover is not null)
                {
                    oldCover.IsCover = false;
                    _mediaRepo.Update(oldCover);
                }
            }

            var media = new RealEstate.Domain.Entities.PropertyMedia
            {
                PropertyId = request.PropertyId,
                MediaUrl = url,
                MediaType = request.MediaType,
                IsCover = request.IsCover,
                SortOrder = request.SortOrder
            };

            await _mediaRepo.AddAsync(media);
            await _mediaRepo.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(media.Id, "تم رفع الملف بنجاح");
        }
    }
}
