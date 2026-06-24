using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Payments.Queries.GetOverduePayments
{
    public record GetOverduePaymentsQuery(Guid CompanyId)
     : IRequest<ApiResponse<List<OverduePaymentDto>>>;

   
}
