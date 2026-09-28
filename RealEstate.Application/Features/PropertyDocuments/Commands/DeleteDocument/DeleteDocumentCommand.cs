using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.PropertyDocuments.Commands.DeleteDocument
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class DeleteDocumentCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid DocumentId { get; init; }
        public Guid PropertyId { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}