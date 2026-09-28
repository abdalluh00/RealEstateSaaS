using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Auth.Commands.ChangePassword
{
    public sealed class ChangePasswordCommand
        : IRequest<ApiResponse<bool>>, IAuthRequest
    {
        public string Token { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}