using MediatR;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest<ApiResponse<bool>>;

}
