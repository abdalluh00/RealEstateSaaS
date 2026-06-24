using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(
    string Email,
    string Password
) : IRequest<ApiResponse<LoginResult>>, IAuthRequest;

    public record LoginResult(
        Guid Id,
        string Token,
        string FullName,
        string Email,
        string Role,
        Guid CompanyId,
        DateTime ExpiresAt
    );
}
