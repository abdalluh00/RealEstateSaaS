using MediatR;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Commands.UploadMaintenanceMedia
{
    [Authorize(Roles = $"{Roles.Agent},{Roles.Admin}")]
    public sealed class UploadMaintenanceMediaCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid MaintenanceRequestId { get; init; }
        public IFormFile File { get; init; } = null!;
        public MediaStage Stage { get; init; } = MediaStage.Before;
        public UploadedByType UploadedByType { get; init; }
        public Guid UploadedById { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}