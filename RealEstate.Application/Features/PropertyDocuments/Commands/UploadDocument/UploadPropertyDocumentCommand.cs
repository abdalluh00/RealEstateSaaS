using MediatR;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.PropertyDocuments.Commands.UploadDocument
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class UploadPropertyDocumentCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid PropertyId { get; init; }
        public IFormFile File { get; init; } = null!;
        public PropertyDocumentType DocumentType { get; init; }
        public string DocumentName { get; init; } = string.Empty;
        public string? DocumentNumber { get; init; }
        public DateTime? IssueDate { get; init; }
        public DateTime? ExpiryDate { get; init; }
        public string? Notes { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}