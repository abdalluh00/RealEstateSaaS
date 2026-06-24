using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Clients.DTO
{
    public record ClientDto(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        string LeadStatus,
        string? Source,
        DateTime CreatedAt
    );
}
