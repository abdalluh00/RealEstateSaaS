using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Owner
{
    public sealed record OwnerListDto
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string OwnerType { get; init; } = string.Empty;
        public string? CompanyName { get; init; }
        public bool IsActive { get; init; }
        public int PropertyCount { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
