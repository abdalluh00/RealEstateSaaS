using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Auth.Commands.ResetPassword
{
    public sealed class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;
        private readonly IWhatsAppService _whatsApp;

        public ResetPasswordCommandHandler(
            IUserRepository users,
            IUnitOfWork uow,
            IWhatsAppService whatsApp)
        {
            _users = users;
            _uow = uow;
            _whatsApp = whatsApp;
        }

        public async Task<ApiResponse<bool>> Handle(
            ResetPasswordCommand cmd,
            CancellationToken ct)
        {
            var user = await _users.GetByEmailAsync(cmd.Email, ct);

            // ── Silent fail — don't reveal if email exists ─
            if (user is null || !user.IsActive)
                return ApiResponse<bool>.Ok(true,
                    "إذا كان البريد مسجلاً سيصلك رمز إعادة التعيين");

            var token = Guid.NewGuid().ToString("N");

            user.PasswordResetToken = token;
            user.PasswordResetExpiry = DateTime.UtcNow.AddHours(1);

            await _uow.SaveChangesAsync(ct);

            await _whatsApp.SendAsync(
                user.Phone,
                $"""
                مرحباً {user.FullName} 👋

                رمز إعادة تعيين كلمة المرور:
                {token}
                صالح لمدة ساعة واحدة فقط.
                """);

            return ApiResponse<bool>.Ok(true,
                "إذا كان البريد مسجلاً سيصلك رمز إعادة التعيين");
        }
    }
}