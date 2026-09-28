using MediatR;
using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;
using RealEstate.Shared.Settings;
using Microsoft.Extensions.Options;

namespace RealEstate.Application.Features.Auth.Commands.AcceptInvitation
{
    public sealed class AcceptInvitationCommandHandler
        : IRequestHandler<AcceptInvitationCommand, ApiResponse<LoginResponseDto>>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwt;
        private readonly JwtSettings _jwtSettings;

        public AcceptInvitationCommandHandler(
            IUserRepository users,
            IUnitOfWork uow,
            IJwtService jwt,
            IOptions<JwtSettings> jwtSettings)
        {
            _users = users;
            _uow = uow;
            _jwt = jwt;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse<LoginResponseDto>> Handle(
            AcceptInvitationCommand cmd,
            CancellationToken ct)
        {
            var user = await _users.GetByInvitationTokenAsync(cmd.Token, ct);

            if (user is null)
                throw new NotFoundException("رمز الدعوة غير صحيح");

            if (user.InvitationExpiry < DateTime.UtcNow)
                throw new ConflictException("انتهت صلاحية رمز الدعوة");

            if (user.IsInvitationAccepted)
                throw new ConflictException("تم قبول الدعوة مسبقاً");

            user.PasswordHash = PasswordHelper.Hash(cmd.Password);
            user.IsInvitationAccepted = true;
            user.IsActive = true;
            user.InvitationAcceptedAt = DateTime.UtcNow;
            user.InvitationToken = null;  // clear token after use
            user.InvitationExpiry = null;
            user.LastLoginAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            var token = _jwt.GenerateToken(user);

            return ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                CompanyId = user.CompanyId,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays)
            });
        }
    }
}