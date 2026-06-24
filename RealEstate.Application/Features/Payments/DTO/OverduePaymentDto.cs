using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Payments.DTO
{
    public record OverduePaymentDto(
       Guid Id,
       string PropertyTitle,
       string ClientName,
       string ClientPhone,
       decimal Amount,
       DateTime DueDate,
       int DaysOverdue,
       Guid ContractId
   );
}
