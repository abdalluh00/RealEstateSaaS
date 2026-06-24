
using FluentValidation;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        private static readonly string[] AllowedRoles = ["Owner", "Admin", "Agent"];

        public CreateUserValidator()
        {
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

            RuleFor(x => x.Role)
             .Must(role => role is UserRole.Owner or UserRole.Admin or UserRole.Agent)
             .WithMessage("الدور يجب أن يكون Owner أو Admin أو Agent");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("معرف الشركة مطلوب");
        }
    }
}
