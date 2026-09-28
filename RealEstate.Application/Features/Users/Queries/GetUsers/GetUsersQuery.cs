using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Users;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Queries.GetPagedUsers
{
    public sealed class GetPagedUsersQuery
        : IRequest<ApiResponse<PagedResult<UserListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public UserRole? Role { get; init; }
        public bool? IsActive { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}