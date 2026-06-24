using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;
namespace RealEstate.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _repo;

        public ChangePasswordHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            ChangePasswordCommand request,
            CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.UserId);

            if (user is null)
                throw new NotFoundException("المستخدم", request.UserId);

            // تحقق من كلمة المرور القديمة
            if (!PasswordHelper.Verify(request.OldPassword, user.PasswordHash))
                throw new ValidationException("كلمة المرور القديمة غير صحيحة");

            user.PasswordHash = PasswordHelper.Hash(request.NewPassword);

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تغيير كلمة المرور بنجاح");
        }
    }
}
