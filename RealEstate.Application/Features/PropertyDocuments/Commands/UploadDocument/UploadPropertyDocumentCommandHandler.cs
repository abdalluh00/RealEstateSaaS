using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.PropertyDocuments.Commands.UploadDocument
{
    public sealed class UploadPropertyDocumentCommandHandler
        : IRequestHandler<UploadPropertyDocumentCommand, ApiResponse<Guid>>
    {
        private readonly IPropertyDocumentRepository _documentRepo;
        private readonly IPropertyRepository _properties;
        private readonly IFileStorageService _storage;
        private readonly IUnitOfWork _uow;

        public UploadPropertyDocumentCommandHandler(
            IPropertyDocumentRepository documentRepo,
            IPropertyRepository properties,
            IFileStorageService storage,
            IUnitOfWork uow)
        {
            _documentRepo = documentRepo;
            _properties = properties;
            _storage = storage;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            UploadPropertyDocumentCommand cmd,
            CancellationToken ct)
        {
            var propertyExists = await _properties.ExistsAsync(
                cmd.PropertyId, cmd.CompanyId, ct);

            if (!propertyExists)
                throw new NotFoundException("العقار", cmd.PropertyId);

            var folder = $"properties/{cmd.PropertyId}/documents";
            var url = await _storage.SaveAsync(
                cmd.File.OpenReadStream(),
                cmd.File.FileName,
                folder,
                ct);

            var document = new PropertyDocument
            {
                PropertyId = cmd.PropertyId,
                CompanyId = cmd.CompanyId,
                DocumentType = cmd.DocumentType,
                DocumentName = cmd.DocumentName,
                FileUrl = url,
                DocumentNumber = cmd.DocumentNumber,
                IssueDate = cmd.IssueDate,
                ExpiryDate = cmd.ExpiryDate,
                Notes = cmd.Notes,
                FileName = cmd.File.FileName,
                FileSizeInBytes = cmd.File.Length,
                MimeType = cmd.File.ContentType
            };

            _documentRepo.Add(document);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(document.Id, "تم رفع المستند بنجاح");
        }
    }
}