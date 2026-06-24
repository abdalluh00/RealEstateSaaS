using MediatR;
using RealEstate.Application.Features.Users.DTO;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IRequest<ApiResponse<UserDetailDto>>;

    
}
