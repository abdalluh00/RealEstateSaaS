using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;

namespace RealEstate.Application.Features.Auth.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;

        public ChangePasswordCommandHandler(
            IUserRepository users,
            IUnitOfWork uow)
        {
            _users = users;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            ChangePasswordCommand cmd,
            CancellationToken ct)
        {
            var user = await _users.GetByPasswordResetTokenAsync(cmd.Token, ct);

            if (user is null)
                throw new NotFoundException("رمز إعادة التعيين غير صحيح");

            if (user.PasswordResetExpiry < DateTime.UtcNow)
                throw new ConflictException("انتهت صلاحية رمز إعادة التعيين");

            user.PasswordHash = PasswordHelper.Hash(cmd.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetExpiry = null;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تغيير كلمة المرور بنجاح");
        }
    }
}