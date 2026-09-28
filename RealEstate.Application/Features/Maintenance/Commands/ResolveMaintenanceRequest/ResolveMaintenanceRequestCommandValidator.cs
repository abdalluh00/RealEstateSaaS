using FluentValidation;

namespace RealEstate.Application.Features.Maintenance.Commands.ResolveMaintenanceRequest
{
    public sealed class ResolveMaintenanceRequestCommandValidator
        : AbstractValidator<ResolveMaintenanceRequestCommand>
    {
        public ResolveMaintenanceRequestCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الطلب مطلوب");

            RuleFor(x => x.ResolutionNotes)
                .NotEmpty().WithMessage("ملاحظات الحل مطلوبة")
                .MaximumLength(1000).WithMessage("الملاحظات لا تتجاوز 1000 حرف");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Cost.HasValue)
                .WithMessage("التكلفة لا يمكن أن تكون سالبة");
        }
    }
}