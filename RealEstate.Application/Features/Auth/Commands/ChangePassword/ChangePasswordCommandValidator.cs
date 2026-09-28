using FluentValidation;

namespace RealEstate.Application.Features.Auth.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandValidator
        : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("رمز إعادة التعيين مطلوب");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("كلمات المرور غير متطابقة");
        }
    }
}