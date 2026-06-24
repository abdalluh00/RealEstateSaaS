using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetFeaturedProperties
{
    public sealed class GetFeaturedPropertiesQueryValidator
        : AbstractValidator<GetFeaturedPropertiesQuery>
    {
        public GetFeaturedPropertiesQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("رقم الصفحة يجب أن يكون 1 أو أكثر");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("حجم الصفحة يجب أن يكون بين 1 و 50");
        }
    }
}
