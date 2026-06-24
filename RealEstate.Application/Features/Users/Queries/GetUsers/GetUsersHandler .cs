using MediatR;
using RealEstate.Application.Features.Users.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, ApiResponse<List<UserDto>>>
    {
        private readonly IUserRepository _repo;

        public GetUsersHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<UserDto>>> Handle(
            GetUsersQuery request,
            CancellationToken ct)
        {
            var users = await _repo.GetByCompanyAsync(request.CompanyId);

            var result = users.Select(u => new UserDto(
                u.Id, u.FullName, u.Email,
                u.Phone, u.Role.ToString(), u.IsActive, u.CreatedAt
            )).ToList();

            return ApiResponse<List<UserDto>>.Ok(result);
        }
    }
}
