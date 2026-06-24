using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetPropertiesByAgent
{
    public sealed class GetPropertiesByAgentQueryValidator
       : AbstractValidator<GetPropertiesByAgentQuery>
    {
        public GetPropertiesByAgentQueryValidator()
        {
            RuleFor(x => x.AgentId)
                .NotEmpty()
                .WithMessage("معرف الوكيل مطلوب");

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("رقم الصفحة يجب أن يكون 1 أو أكثر");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("حجم الصفحة يجب أن يكون بين 1 و 100");
        }
    }
}
