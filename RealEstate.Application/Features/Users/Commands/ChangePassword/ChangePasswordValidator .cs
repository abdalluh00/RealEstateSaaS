using FluentValidation;
namespace RealEstate.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("كلمة المرور القديمة مطلوبة");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل")
                .Matches(@"[A-Z]").WithMessage("يجب أن تحتوي على حرف كبير")
                .Matches(@"[0-9]").WithMessage("يجب أن تحتوي على رقم")
                .NotEqual(x => x.OldPassword).WithMessage("كلمة المرور الجديدة يجب أن تختلف عن القديمة");
        }
    }
}
