using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Users;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Auth.Commands.AcceptInvitation
{
    public sealed class AcceptInvitationCommand
        : IRequest<ApiResponse<LoginResponseDto>>, IAuthRequest
    {
        public string Token { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}