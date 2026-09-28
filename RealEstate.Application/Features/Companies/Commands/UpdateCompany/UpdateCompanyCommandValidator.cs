using FluentValidation;

namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    public sealed class UpdateCompanyCommandValidator
        : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الشركة مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يتجاوز 200 حرف");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب");
        }
    }
}