using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Auth.Commands.ResetPassword
{
    public sealed class ResetPasswordCommand
        : IRequest<ApiResponse<bool>>, IAuthRequest
    {
        public string Email { get; init; } = string.Empty;
    }
}