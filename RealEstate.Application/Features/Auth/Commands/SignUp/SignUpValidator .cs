using FluentValidation;

namespace RealEstate.Application.Features.Auth.Commands.SignUp
{
    public class SignUpValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpValidator()
        {
            // بيانات الشركة
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("اسم الشركة مطلوب")
                .MaximumLength(200);

            RuleFor(x => x.CompanyPhone)
                .NotEmpty().WithMessage("رقم جوال الشركة مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            // بيانات المالك
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("الاسم مطلوب")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل")
                .Matches(@"[A-Z]").WithMessage("يجب أن تحتوي على حرف كبير")
                .Matches(@"[0-9]").WithMessage("يجب أن تحتوي على رقم");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تأكيد كلمة المرور مطلوب")
                .Equal(x => x.Password).WithMessage("كلمة المرور غير متطابقة");
        }
    }
}
