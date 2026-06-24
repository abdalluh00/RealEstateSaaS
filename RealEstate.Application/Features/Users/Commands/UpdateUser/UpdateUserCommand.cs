using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
     Guid Id,
     string FullName,
     string Phone,
     UserRole Role,
     bool IsActive
 ) : IRequest<ApiResponse<bool>>;
}
