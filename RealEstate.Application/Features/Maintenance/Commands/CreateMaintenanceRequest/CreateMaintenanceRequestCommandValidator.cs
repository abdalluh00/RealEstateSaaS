using FluentValidation;

namespace RealEstate.Application.Features.Maintenance.Commands.CreateMaintenanceRequest
{
    public sealed class CreateMaintenanceRequestCommandValidator
        : AbstractValidator<CreateMaintenanceRequestCommand>
    {
        public CreateMaintenanceRequestCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الطلب مطلوب")
                .MaximumLength(200).WithMessage("العنوان لا يتجاوز 200 حرف");

            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("تصنيف الطلب غير صحيح");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("أولوية الطلب غير صحيحة");
        }
    }
}