using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Payments.Queries.GetPaymentSummary
{
    public record GetPaymentSummaryQuery(Guid CompanyId)
    : IRequest<ApiResponse<PaymentSummaryDto>>;

   
}
