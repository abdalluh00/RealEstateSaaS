using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia
{
    public class DeleteMediaHandler : IRequestHandler<DeleteMediaCommand, ApiResponse<bool>>
    {
        private readonly IPropertyMediaRepository _repo;
        private readonly IStorageService _storage;

        public DeleteMediaHandler(
            IPropertyMediaRepository repo,
            IStorageService storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteMediaCommand request,
            CancellationToken ct)
        {
            var media = await _repo.GetByIdAsync(request.Id);

            if (media is null)
                throw new NotFoundException("الملف", request.Id);

            // احذف الملف من السيرفر
            await _storage.DeleteAsync(media.MediaUrl);

            // احذف من قاعدة البيانات
            _repo.Delete(media);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف الملف بنجاح");
        }
    }
}
