using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Owners.DTO
{
    public record OwnerPropertyDto(
       Guid Id,
       string Title,
       string Type,
       string Status,
       decimal Price,
       string City
   );
}
