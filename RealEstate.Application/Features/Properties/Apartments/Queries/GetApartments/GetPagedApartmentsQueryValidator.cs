using FluentValidation;
using RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments;

namespace RealEstate.Application.Features.Apartments.Queries.GetApartments
{
    public sealed class GetPagedApartmentsQueryValidator
          : AbstractValidator<GetPagedApartmentsQuery>
    {
        public GetPagedApartmentsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("رقم الصفحة يجب أن يكون 1 أو أكثر");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("حجم الصفحة يجب أن يكون بين 1 و 100");

            RuleFor(x => x.MinBedrooms)
                .GreaterThan(0).When(x => x.MinBedrooms.HasValue)
                .WithMessage("الحد الأدنى لغرف النوم يجب أن يكون أكبر من صفر");

            RuleFor(x => x.MaxBedrooms)
                .GreaterThanOrEqualTo(x => x.MinBedrooms!.Value)
                .When(x => x.MinBedrooms.HasValue && x.MaxBedrooms.HasValue)
                .WithMessage("الحد الأقصى يجب أن يكون أكبر من أو يساوي الحد الأدنى");
        }
    }
}