using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class DeleteMediaCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid MediaId { get; init; }
        public Guid PropertyId { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}