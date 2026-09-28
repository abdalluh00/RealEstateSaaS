using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Users;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Auth.Commands.Login
{
    public sealed class LoginCommand
        : IRequest<ApiResponse<LoginResponseDto>>, IAuthRequest
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}