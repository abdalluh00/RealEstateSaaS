using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Commands.InviteUser
{
    [Authorize(Roles = $"{Roles.Admin},{Roles.Owner}")]
    public sealed class InviteUserCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}