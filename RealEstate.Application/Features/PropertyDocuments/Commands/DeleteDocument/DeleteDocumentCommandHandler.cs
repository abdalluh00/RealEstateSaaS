using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyDocuments.Commands.DeleteDocument
{
    public sealed class DeleteDocumentCommandHandler
        : IRequestHandler<DeleteDocumentCommand, ApiResponse<bool>>
    {
        private readonly IPropertyDocumentRepository _documentRepo;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public DeleteDocumentCommandHandler(
            IPropertyDocumentRepository documentRepo,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _documentRepo = documentRepo;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteDocumentCommand cmd,
            CancellationToken ct)
        {
            var document = await _documentRepo.GetByIdForCommandAsync(
                cmd.DocumentId, cmd.CompanyId, ct);

            if (document is null || document.PropertyId != cmd.PropertyId)
                throw new NotFoundException("المستند", cmd.DocumentId);

            await _storage.DeleteAsync(document.FileUrl, ct);

            _documentRepo.HardDelete(document);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف المستند بنجاح");
        }
    }
}