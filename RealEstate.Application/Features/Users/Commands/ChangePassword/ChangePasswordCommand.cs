using MediatR;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
    Guid UserId,
    string OldPassword,
    string NewPassword
) : IRequest<ApiResponse<bool>>;
}
