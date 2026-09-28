using MediatR;
using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Queries.GetPagedUsers
{
    public sealed class GetPagedUsersQueryHandler
        : IRequestHandler<GetPagedUsersQuery, ApiResponse<PagedResult<UserListDto>>>
    {
        private readonly IUserRepository _users;

        public GetPagedUsersQueryHandler(IUserRepository users)
            => _users = users;

        public async Task<ApiResponse<PagedResult<UserListDto>>> Handle(
            GetPagedUsersQuery query,
            CancellationToken ct)
        {
            var result = await _users.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                role: query.Role,
                isActive: query.IsActive,
                ct: ct);

            return ApiResponse<PagedResult<UserListDto>>.Ok(result);
        }
    }
}