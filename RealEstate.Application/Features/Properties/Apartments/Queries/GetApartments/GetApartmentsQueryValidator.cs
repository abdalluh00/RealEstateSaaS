using FluentValidation;
using RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments;

namespace RealEstate.Application.Features.Apartments.Queries.GetApartments
{
    public class GetApartmentsValidator : AbstractValidator<GetApartmentsQuery>
    {
        public GetApartmentsValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("رقم الصفحة يجب أن يكون أكبر من 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("حجم الصفحة يجب أن يكون بين 1 و 100");
        }
    }
}