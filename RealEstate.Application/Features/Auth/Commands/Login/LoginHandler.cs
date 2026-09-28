using MediatR;
using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;
using RealEstate.Shared.Settings;
using Microsoft.Extensions.Options;

namespace RealEstate.Application.Features.Auth.Commands.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto>>
    {
        private readonly IUserRepository _users;
        private readonly ICompanyRepository _companies;
        private readonly IJwtService _jwt;
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUserRepository users,
            ICompanyRepository companies,
            IJwtService jwt,
            IUnitOfWork uow,
            IOptions<JwtSettings> jwtSettings)
        {
            _users = users;
            _companies = companies;
            _jwt = jwt;
            _uow = uow;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse<LoginResponseDto>> Handle(
            LoginCommand cmd,
            CancellationToken ct)
        {
            // ── Find user ─────────────────────────────────
            var user = await _users.GetByEmailAsync(cmd.Email, ct);

            if (user is null || !PasswordHelper.Verify(cmd.Password, user.PasswordHash))
                throw new UnauthorizedException("البريد الإلكتروني أو كلمة المرور غير صحيحة");

            // ── Must have accepted invitation ─────────────
            if (!user.IsInvitationAccepted)
                throw new UnauthorizedException("يجب قبول الدعوة أولاً");

            // ── Must be active ────────────────────────────
            if (!user.IsActive)
                throw new ForbiddenException("الحساب معطل، تواصل مع المسؤول");

            // ── Company must be active + subscription valid ─
            var subscriptionValid = await _companies
                .IsSubscriptionValidAsync(user.CompanyId, ct);

            if (!subscriptionValid)
                throw new ForbiddenException("اشتراك الشركة منتهي، تواصل مع مزود الخدمة");

            // ── Stamp last login ──────────────────────────
            user.LastLoginAt = DateTime.UtcNow;
            _users.Update(user);
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