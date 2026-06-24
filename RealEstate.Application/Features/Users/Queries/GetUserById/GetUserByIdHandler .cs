using MediatR;
using RealEstate.Application.Features.Users.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;


namespace RealEstate.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<UserDetailDto>>
    {
        private readonly IUserRepository _repo; // ← Interface فقط

        public GetUserByIdHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<UserDetailDto>> Handle(
            GetUserByIdQuery request,
            CancellationToken ct)
        {
            var user = await _repo.GetWithCompanyAsync(request.Id);

            if (user is null)
                throw new NotFoundException("المستخدم", request.Id);

            var result = new UserDetailDto(
                user.Id,
                user.FullName,
                user.Email,
                user.Phone,
                user.Role.ToString(),
                user.IsActive,
                user.CompanyId,
                user.CompanyName,
                user.CreatedAt
            );

            return ApiResponse<UserDetailDto>.Ok(result);
        }
    }
}
