using MediatR;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class UploadPropertyMediaCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid PropertyId { get; init; }
        public IFormFile File { get; init; } = null!;
        public string? Title { get; init; }
        public string? Description { get; init; }
        public int SortOrder { get; init; } = 0;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}